using CBDCHubContract.Contracts.HubContract;
using CBDCHubContract.Contracts.HubContract.ContractDefinition;
using ITSTILoopLibrary.UtilityServices;
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

    public class CBDCBridgeEventWatcherConfig
    {
        public CBDCBridgeEventWatcherConfig()
        {
        }

        public string Address { get; set; } = String.Empty;
        public string Key { get; set; } = String.Empty;
        public string TransactionHash { get; set; } = String.Empty;
        public string RpcEndpoint { get; set; } = String.Empty;
        public int NetworkId { get; set; } = 0;
    }

    public class CBDCHubService
    {
        private readonly ILogger<CBDCHubService> _logger;
        private List<RegisteredFspDTO> _registeredFspDTOs = new List<RegisteredFspDTO>();
        private List<AccountFundedEventDTO> _fundedAccounts = new List<AccountFundedEventDTO>();        
        private readonly Web3 _web3;
        private readonly HubContractService _hubContractService;

        public CBDCHubService(ILogger<CBDCHubService> logger, EthereumEventRetriever ethereumEventRetriever, IOptions<CBDCBridgeEventWatcherConfig> config)
        {
            _logger = logger;
            IClient client = new RpcClient(new Uri(config.Value.RpcEndpoint));
            _web3 = new Web3(new Account(config.Value.Key, config.Value.NetworkId), client);
            _web3.TransactionManager.UseLegacyAsDefault = true;
            _hubContractService = new HubContractService(_web3, config.Value.Address);
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
            var bigBalance = await _hubContractService.GetFSPBalanceQueryAsync(fspAddress);
            bigBalance = bigBalance / (BigInteger) Math.Pow(10, 18);
            return (decimal) bigBalance;
        }

        public async Task SettleAccountsAsync(int settlementId, Dictionary<string, decimal> netSettlementValues)
        {
            List<string> accounts = new List<string>();
            List<BigInteger> positions = new List<BigInteger>();
            foreach (KeyValuePair<string, decimal> kvp in netSettlementValues)
            {
                accounts.Add(kvp.Key);
                BigInteger bigPosition = (BigInteger)kvp.Value;
                bigPosition = bigPosition * (BigInteger) Math.Pow(10, 18);
                positions.Add(bigPosition);
            }

            var result = await _hubContractService.MultilateralSettlementRequestAsync(settlementId, accounts, positions);
        }



    }
}
