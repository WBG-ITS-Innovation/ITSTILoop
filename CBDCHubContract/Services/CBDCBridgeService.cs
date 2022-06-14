using CBDCHubContract.Contracts.HubContract;
using CBDCHubContract.Contracts.HubContract.ContractDefinition;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        private readonly IOptions<EthereumConfig> _config;
        private readonly Web3 _web3;
        private readonly HubContractService _hubContractService;

        public CBDCBridgeService(ILogger<CBDCBridgeService> logger, EthereumEventRetriever ethereumEventRetriever, IOptions<EthereumConfig> config)
        {
            _ethereumEventRetriever = ethereumEventRetriever;
            _fspRegistrationHandler = _ethereumEventRetriever.CreateEventHandler<FSPRegistrationEventDTO>();
            _accountFunded = _ethereumEventRetriever.CreateEventHandler<AccountFundedEventDTO>();
            _settlement = _ethereumEventRetriever.CreateEventHandler<SettlementEventDTO>();
            _fspPayout = _ethereumEventRetriever.CreateEventHandler<FSPpayoutEventDTO>();
            _fspDebt = _ethereumEventRetriever.CreateEventHandler<FSPdebtEventDTO>();
            _logger = logger;
            _config = config;
            IClient client = new RpcClient(new Uri(config.Value.RpcEndpoint));
            _web3 = new Web3(new Account(config.Value.ContractOwnerKey, 1492), client);
            _web3.TransactionManager.UseLegacyAsDefault = true;
            _hubContractService = new HubContractService(_web3, _config.Value.ContractAddress);
        }

        public async Task<Dictionary<string, decimal>> CheckBalancesAsync(List<string> addresses)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            foreach (string address in addresses)
            {
                var balance = await CheckBalanceAsync(address);
                result.Add(address, balance);
            }
            return result;
        }

        public async Task<decimal> CheckBalanceAsync(string fspAddress)
        {
            Decimal balance = 0;
            balance = (decimal)await _hubContractService.GetFSPBalanceQueryAsync(fspAddress);
            return balance;
        }

        public async Task SettleAccountsAsync(int settlementId, Dictionary<string, decimal> netSettlementValues)
        {
            List<string> accounts = new List<string>();
            List<BigInteger> positions = new List<BigInteger>();
            foreach (KeyValuePair<string, decimal> kvp in netSettlementValues)
            {
                accounts.Add(kvp.Key);
                positions.Add((BigInteger)kvp.Value);
            }
            var result = await _hubContractService.MultilateralSettlementRequestAsync(settlementId, accounts, positions);
        }



    }
}
