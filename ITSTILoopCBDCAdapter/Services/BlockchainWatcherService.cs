using System;
using System.Numerics;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Cbdchubcontract.Contracts.CbTransferContract.ContractDefinition;
using ITSTILoopDTOLibrary;
using System.Text;
using Cbdchubcontract.Contracts.CbTransferContract;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Model;
using ITSTILoopLibrary.UtilityServices;
using ITSTILoopLibrary.DTO;
using ITSTILoopLibrary.UtilityServices.Interfaces;
using Microsoft.Extensions.Options;
using CBDCTransferContract;

namespace ITSTILoopCBDCAdapter.Services
{
    public class BlockchainWatcherService : BackgroundService
    {
        private readonly ILogger<BlockchainWatcherService> _logger;
        private readonly EthereumEventRetriever _ethereumEventRetriever;
        private readonly IHttpPostClient _httpPostClient;
        private readonly CbTransferContractService _cbTransferContractService;
        private readonly Web3 _web3;
        private Event<TransferCreatedEventDTO>? _transferCreatedEvent;
        private Event<TransferSuccessfulEventDTO>? _transferSuccesfullEvent;
        private Event<TransferErrorEventDTO>? _transferErrorEvent;
        private Event<TransferFailEventDTO>? _transferFailEvent;


        public BlockchainWatcherService(ILogger<BlockchainWatcherService> logger, EthereumEventRetriever ethereumEventRetriever, IHttpPostClient httpPostClient, IOptions<CBDCTransferConfig> config)
        {
            _logger = logger;
            _ethereumEventRetriever = ethereumEventRetriever;
            _ethereumEventRetriever.Config = new EthereumConfig() { ContractAddress = config.Value.Address, ContractOwnerKey = config.Value.Key, NetworkId = config.Value.NetworkId, RpcEndpoint = config.Value.RpcEndpoint };
            _httpPostClient = httpPostClient;
            IClient client = new RpcClient(new Uri(config.Value.RpcEndpoint));
            _web3 = new Web3(new Nethereum.Web3.Accounts.Account(config.Value.OwnerKey, config.Value.NetworkId), client);
            _web3.TransactionManager.UseLegacyAsDefault = true;
            _cbTransferContractService = new CbTransferContractService(_web3, config.Value.Address);
            CreateEventHandlers();
        }

        public void CreateEventHandlers()
        {
            _transferCreatedEvent = _ethereumEventRetriever.CreateEventHandler<TransferCreatedEventDTO>();
            _transferSuccesfullEvent = _ethereumEventRetriever.CreateEventHandler<TransferSuccessfulEventDTO>();
            _transferErrorEvent = _ethereumEventRetriever.CreateEventHandler<TransferErrorEventDTO>();
            _transferFailEvent = _ethereumEventRetriever.CreateEventHandler<TransferFailEventDTO>();
        }

        public async Task ProcessTransferErrorLogsAsync(List<EventLog<TransferErrorEventDTO>> eventLogs)
        {
            try
            {
                foreach (var transferErrorEvent in eventLogs)
                {
                    _logger.LogInformation($"{transferErrorEvent.Event.Reason}");
                }
            }
            catch (Exception ex)
            {

            }
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
                    _logger.LogInformation($"ProcessTransferCreatedLogsAsync-{transferCreatedEvent.Event.From}-{transferCreatedEvent.Event.To}-{transferCreatedEvent.Event.Amount}");
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
                    else
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
                await _ethereumEventRetriever.RetrievePastLogsAsync<TransferErrorEventDTO>(_transferErrorEvent, ProcessTransferErrorLogsAsync, start, end);
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



