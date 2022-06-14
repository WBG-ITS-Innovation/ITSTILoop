using CBDCHubContract.Contracts.HubContract.ContractDefinition;
using CBDCHubContract.Services;
using ITSTILoop.Context.Repositories.Interfaces;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;

namespace ITSTILoop.Services
{
    public class CBDCBridgeEventWatcherService : BackgroundService
    {
        private readonly ILogger<CBDCBridgeEventWatcherService> _logger;
        private readonly EthereumEventRetriever _ethereumEventRetriever;
        private readonly IParticipantRepository _participantRepository;
        private readonly ISettlementWindowRepository _settlementWindowRepository;
        private Event<AccountFundedEventDTO> _accountFunded;
        private Event<MultilateralSettlementEventDTO> _settlement;
        private Event<FSPpayoutEventDTO> _fspPayout;
        private Event<FSPdebtEventDTO> _fspDebt;

        public CBDCBridgeEventWatcherService(ILogger<CBDCBridgeEventWatcherService> logger, EthereumEventRetriever ethereumEventRetriever, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _ethereumEventRetriever = ethereumEventRetriever;
            _participantRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IParticipantRepository>();
            _settlementWindowRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ISettlementWindowRepository>();
            _accountFunded = _ethereumEventRetriever.CreateEventHandler<AccountFundedEventDTO>();
            _settlement = _ethereumEventRetriever.CreateEventHandler<MultilateralSettlementEventDTO>();
            _fspPayout = _ethereumEventRetriever.CreateEventHandler<FSPpayoutEventDTO>();
            _fspDebt = _ethereumEventRetriever.CreateEventHandler<FSPdebtEventDTO>();
        }

        public async Task BlockHandler(BlockParameter start, BlockParameter end)
        {
            _logger.LogInformation($"BlockHandler-{start}-{end}");
            try
            {
                //await _ethereumEventRetriever.RetrievePastLogsAsync<CoopAddedEventDTO>(_coopHandler, ProcessCoopAddedLogsAsync, start, end);
                //await _ethereumEventRetriever.RetrievePastLogsAsync<VendorAddedEventDTO>(_vendorHandler, ProcessVendorAddedLogsAsync, start, end);
                await _ethereumEventRetriever.RetrievePastLogsAsync<AccountFundedEventDTO>(_accountFunded, ProcessAccountFundedAsync, start, end);
                await _ethereumEventRetriever.RetrievePastLogsAsync<MultilateralSettlementEventDTO>(_settlement, ProcessSettlementAsync, start, end);
                //await _ethereumEventRetriever.RetrievePastLogsAsync<FSPpayoutEventDTO>(_fspPayout, ProcessFspPayoutAsync, start, end);
                //await _ethereumEventRetriever.RetrievePastLogsAsync<FSPdebtEventDTO>(_fspDebt, ProcessFspDebtAsync, start, end);
            }
            catch (Exception ex)
            {
                _logger.LogError($"BlockHandler-EX-{ex}");
            }
        }

        private async Task ProcessFspDebtAsync(List<EventLog<FSPdebtEventDTO>> arg)
        {
            _logger.LogInformation($"ProcessFspDebtAsync-{arg.Count}");
            //throw new NotImplementedException();
        }

        private async Task ProcessFspPayoutAsync(List<EventLog<FSPpayoutEventDTO>> arg)
        {
            _logger.LogInformation($"ProcessFspPayoutAsync-{arg.Count}");
            //throw new NotImplementedException();
        }

        private async Task ProcessSettlementAsync(List<EventLog<MultilateralSettlementEventDTO>> arg)
        {
            _logger.LogInformation($"ProcessSettlementAsync-{arg.Count}");
            foreach (var log in arg)
            {
                _logger.LogInformation($"ProcessSettlementAsync-{log.Event.SettlementId}");
                _settlementWindowRepository.SettleSettlementWindow();
            }
        }

        private async Task ProcessAccountFundedAsync(List<EventLog<AccountFundedEventDTO>> arg)
        {
            _logger.LogInformation($"ProcessAccountFundedAsync-{arg.Count}");
            foreach (var log in arg)
            {
                _logger.LogInformation($"ProcessAccountFundedAsync-{log.Event.Fsp}");
                _participantRepository.FundParticipant(log.Event.Fsp, (decimal) log.Event.Tokens);                
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CBDCBridgeEventWatcherService started");
            var latestBlock = await _ethereumEventRetriever.GetLatestBlockAsync();
            await _ethereumEventRetriever.CheckForNewLogsAsync(BlockHandler, latestBlock, stoppingToken);
        }
    }
}
