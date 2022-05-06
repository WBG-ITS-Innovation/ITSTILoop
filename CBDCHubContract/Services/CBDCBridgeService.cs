using CBDCFPShubContract.Contracts.HubContract.ContractDefinition;
using Microsoft.Extensions.Logging;
using Nethereum.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBDCHubContract.Services
{
    public class RegisteredFspDTO
    {
        public string RegisteredFspId { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
    }

    public class CBDCBridgeService
    {
        private readonly EthereumEventRetriever _ethereumEventRetriever;
        private Event<FSPRegistrationEventDTO> _fspRegistrationHandler;
        private Event<AccountFundedEventDTO> _accountFunded;
        private Event<SettlementEventDTO> _settlement;
        private Event<FSPpayoutEventDTO> _fspPayout;
        private readonly Event<FSPdebtEventDTO> _fspDebt;
        private readonly ILogger<CBDCBridgeService> _logger;
        private List<RegisteredFspDTO> _registeredFspDTOs = new List<RegisteredFspDTO>();
        private List<AccountFundedEventDTO> _fundedAccounts = new List<AccountFundedEventDTO>();

        public CBDCBridgeService(ILogger<CBDCBridgeService> logger, EthereumEventRetriever ethereumEventRetriever)
        {
            _ethereumEventRetriever = ethereumEventRetriever;
            _fspRegistrationHandler = _ethereumEventRetriever.CreateEventHandler<FSPRegistrationEventDTO>();
            _accountFunded = _ethereumEventRetriever.CreateEventHandler<AccountFundedEventDTO>();
            _settlement = _ethereumEventRetriever.CreateEventHandler<SettlementEventDTO>();
            _fspPayout = _ethereumEventRetriever.CreateEventHandler<FSPpayoutEventDTO>();
            _fspDebt = _ethereumEventRetriever.CreateEventHandler<FSPdebtEventDTO>();
            _logger = logger;
        }

        public async Task<List<RegisteredFspDTO>> GetRegisteredFSPAsync()
        {
            _registeredFspDTOs.Clear();
            await _ethereumEventRetriever.RetrievePastLogsAsync<FSPRegistrationEventDTO>(_fspRegistrationHandler, ProcessRegisteredFSPAsync, 0);
            return _registeredFspDTOs;
        }

        public async Task<List<AccountFundedEventDTO>> GetFundEventsAsync()
        {
            _fundedAccounts.Clear();
            await _ethereumEventRetriever.RetrievePastLogsAsync<AccountFundedEventDTO>(_accountFunded, ProcessAccountFundedAsync, 0);
            return _fundedAccounts;
        }

        public async Task ProcessAccountFundedAsync(List<EventLog<AccountFundedEventDTO>> eventLogs)
        {
            try
            {
                foreach (var accountFundedEvent in eventLogs)
                {
                    _fundedAccounts.Add(accountFundedEvent.Event);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProcessAccountFundedAsync-Error-{ex}");
            }
        }

        public async Task ProcessRegisteredFSPAsync(List<EventLog<FSPRegistrationEventDTO>> eventLogs)
        {
            try
            {
                foreach(var fspRegisteredEvent in eventLogs)
                {
                    RegisteredFspDTO registeredFspDTO = new RegisteredFspDTO();
                    registeredFspDTO.Address = fspRegisteredEvent.Event.Addr;
                    registeredFspDTO.Name = fspRegisteredEvent.Event.Name;
                    registeredFspDTO.RegisteredFspId = fspRegisteredEvent.Event.Id;
                    _registeredFspDTOs.Add(registeredFspDTO);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"ProcessRegisteredFSPAsync-Error-{ex}");
            }
        }

        public async Task RegisterFSP(string name, string address, int id)
        {

        }
    }
}
