using System;
using System.Numerics;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Cbdchubcontract.Contracts.CbTransferContract.ContractDefinition;
using ITSTILoopLibrary.SampleFSPServices;
using ITSTILoopDTOLibrary;
using System.Text;
using Cbdchubcontract.Contracts.CbTransferContract;

namespace ITSTILoopCBDCAdapter.Services
{
    public class BlockchainWatcherService : BackgroundService
    {
        private readonly ILogger<BlockchainWatcherService> _logger;
        private readonly EthereumEventRetriever _ethereumEventRetriever;
        private readonly IHttpPostClient _httpPostClient;
        private readonly CbTransferContractService _cbTransferContractService;
        private Event<TransferCreatedEventDTO>? _transferCreatedEvent;
        private Event<TransferSuccessfulEventDTO>? _transferSuccesfullEvent;
        private Event<TransferErrorEventDTO>? _transferErrorEvent;
        private Event<TransferFailEventDTO>? _transferFailEvent;


        public BlockchainWatcherService(ILogger<BlockchainWatcherService> logger, EthereumEventRetriever ethereumEventRetriever, IServiceProvider serviceProvider, IHttpPostClient httpPostClient, CbTransferContractService cbTransferContractService)
        {
            _logger = logger;
            _ethereumEventRetriever = ethereumEventRetriever;
            _httpPostClient = httpPostClient;
            _cbTransferContractService = cbTransferContractService;
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
                    PartyIdentifierDTO from = new PartyIdentifierDTO() { Identifier = transferCreatedEvent.Event.From, PartyIdentifierType = PartyIdTypes.MSISDN };
                    PartyIdentifierDTO to = new PartyIdentifierDTO() { Identifier = transferCreatedEvent.Event.To, PartyIdentifierType = PartyIdTypes.MSISDN };
                    TransferRequestDTO transferRequestDTO = new TransferRequestDTO() { Amount = ((decimal)transferCreatedEvent.Event.Amount), From = from, To = to, Currency = CurrencyCodes.USD, HomeTransactionId = Guid.NewGuid(), Note = BitConverter.ToString(transferCreatedEvent.Event.TransferHash) };
                    //make the transfer                        
                    var response = await _httpPostClient.PostAsync<TransferRequestDTO, TransferRequestResponseDTO>(transferRequestDTO, "/Transfer", "itstiloop");
                    if (response.Result == HttpPostClientResults.Success)
                    {
                        //auto confirm the transfer
                        var transferId = response.ResponseContent.TransferId;
                        TransferAcceptRejectDTO transferAcceptRejectDTO = new TransferAcceptRejectDTO() { AcceptTransfer = true };
                        var response2 = await _httpPostClient.PostAsync<TransferAcceptRejectDTO, TransferRequestCompleteDTO>(transferAcceptRejectDTO, $"/Transfer/{transferId}", "itstiloop");
                        if (response.Result == HttpPostClientResults.Success)
                        {
                            _logger.LogInformation($"ProcessTransferCreatedLogsAsync-{transferId}-{transferRequestDTO.Note}-Success");
                            var transactionReceipt = await _cbTransferContractService.TransferCompleteRequestAndWaitForReceiptAsync(transferCreatedEvent.Event.TransferHash);
                            _logger.LogInformation($"ProcessTransferCreatedLogsAsync-{transferId}-{transferRequestDTO.Note}-{transactionReceipt.BlockNumber}");
                        }
                        else
                        {
                            var transactionReceipt = await _cbTransferContractService.TransferFailRequestAndWaitForReceiptAsync(transferCreatedEvent.Event.TransferHash);
                            _logger.LogInformation($"ProcessTransferCreatedLogsAsync-TransferFail-{transferId}-{transferRequestDTO.Note}-{transactionReceipt.BlockNumber}");
                        }
                    }
                    {
                        var transactionReceipt = await _cbTransferContractService.TransferFailRequestAndWaitForReceiptAsync(transferCreatedEvent.Event.TransferHash);
                        _logger.LogInformation($"ProcessTransferCreatedLogsAsync-TransferFail-{transferRequestDTO.Note}-{transactionReceipt.BlockNumber}");
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



