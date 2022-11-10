using System;
using System.Numerics;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Cbdchubcontract.Contracts.CbTransferContract.ContractDefinition;
using ITSTILoopLibrary.SampleFSPServices;
using ITSTILoopDTOLibrary;

namespace ITSTILoopCBDCAdapter.Services
{
    public class BlockchainWatcherService : BackgroundService
    {
        private readonly ILogger<BlockchainWatcherService> _logger;
        private readonly EthereumEventRetriever _ethereumEventRetriever;
        private readonly IHttpPostClient _httpPostClient;
        private Event<TransferCreatedEventDTO>? _transferCreatedEvent;
        private Event<TransferSuccessfulEventDTO>? _transferSuccesfullEvent;
        private Event<TransferErrorEventDTO>? _transferErrorEvent;
        private Event<TransferFailEventDTO>? _transferFailEvent;


        public BlockchainWatcherService(ILogger<BlockchainWatcherService> logger, EthereumEventRetriever ethereumEventRetriever, IServiceProvider serviceProvider, IHttpPostClient httpPostClient)
        {
            _logger = logger;
            _ethereumEventRetriever = ethereumEventRetriever;
            _httpPostClient = httpPostClient;
            CreateEventHandlers();
        }

        public void CreateEventHandlers()
        {
            _transferCreatedEvent = _ethereumEventRetriever.CreateEventHandler<TransferCreatedEventDTO>();
            _transferSuccesfullEvent = _ethereumEventRetriever.CreateEventHandler<TransferSuccessfulEventDTO>();
            _transferErrorEvent = _ethereumEventRetriever.CreateEventHandler<TransferErrorEventDTO>();
            _transferFailEvent = _ethereumEventRetriever.CreateEventHandler<TransferFailEventDTO>();
        }

        public async Task ProcessTransferCreatedLogsAsync(List<EventLog<TransferCreatedEventDTO>> eventLogs)
        {
            try
            {
                foreach (var transferCreatedEvent in eventLogs)
                {
                    //Transfer
                    TransferRequestDTO transferRequestDTO = new TransferRequestDTO() { Amount = (decimal)transferCreatedEvent.Event.Amount}
                    //make the transfer                        
                    var response = await _httpPostClient.PostAsync<TransferRequestDTO, TransferRequestResponseDTO>(transferRequestDTO, "/Transfer", "itstiloop");
                    if (response.Result == HttpPostClientResults.Success)
                    {     
                        //auto confirm the transfer
                        //return response.ResponseContent;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProcessProposalAwardedLogsAsync-EX-{ex}");
            }
        }

        public async Task BlockHandler(BlockParameter start, BlockParameter end)
        {
            try
            {
                await _ethereumEventRetriever.RetrievePastLogsAsync<TransferCreatedEventDTO>(_transferCreatedEvent, ProcessTransferCreatedLogsAsync, start, end);
            }
            catch (Exception ex)
            {
                _logger.LogError($"BlockHandler-EX-{ex}");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BlockChainWatcherService started");
            var latestBlock = await _ethereumEventRetriever.GetLatestBlockAsync();
            await _ethereumEventRetriever.CheckForNewLogsAsync(BlockHandler, latestBlock, stoppingToken);

        }
    }
}



