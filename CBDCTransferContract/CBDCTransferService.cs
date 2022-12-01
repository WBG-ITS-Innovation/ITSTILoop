using Cbdchubcontract.Contracts.CbTransferContract;
using ITSTILoopLibrary.Utility;
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

namespace CBDCTransferContract
{
    public enum TransferSourceDestinationType { Hub, Bank, CBDC};

    public class CBDCTransferConfig
    {
        public CBDCTransferConfig()
        {
        }

        public string Address { get; set; } = String.Empty;
        public string Key { get; set; } = String.Empty;
        public string OwnerKey { get; set; } = String.Empty;
        public string TransactionHash { get; set; } = String.Empty;
        public string RpcEndpoint { get; set; } = String.Empty;
        public int NetworkId { get; set; } = 0;
    }

    public class CBDCTransferService
    {
        private readonly ILogger<CBDCTransferService> _logger;

        private readonly Web3 _web3;
        private readonly CbTransferContractService _cbTransferContractService;

        public CBDCTransferService(ILogger<CBDCTransferService> logger, IOptions<CBDCTransferConfig> config)
        {
            _logger = logger;

            IClient client = new RpcClient(new Uri(config.Value.RpcEndpoint));
            _web3 = new Web3(new Account(config.Value.Key,config.Value.NetworkId), client);
            _web3.TransactionManager.UseLegacyAsDefault = true;
            _cbTransferContractService = new CbTransferContractService(_web3, config.Value.Address );
        }

        public async Task<string> MakeTransfer(string from, string to, int amount, string destination, TransferSourceDestinationType destinationType = TransferSourceDestinationType.Hub )
        {
            try
            {
                var txReceipt = await _cbTransferContractService.MakeTransferRequestAndWaitForReceiptAsync(from, to, amount, destination, (byte)destinationType);
                return txReceipt.TransactionHash;
            }
            catch (Exception ex)
            {
                _logger.LogError($"MakeTransfer-EX-{ex}");
            }
            return String.Empty;
        }

        public async Task<string> MakeCBDCTransfer(string from, string to, int amount, string source, TransferSourceDestinationType sourceType = TransferSourceDestinationType.Hub)
        {
            _logger.LogInformation($"MakeCBDCTransfer-ENTRY-{from}-{to}-{amount}");
            try
            {
                var txReceipt = await _cbTransferContractService.MakeCBDCTransferRequestAndWaitForReceiptAsync(from, to, amount, source, (byte)sourceType);
                _logger.LogInformation($"MakeCBDCTransfer-SUCCESS-{from}-{to}-{amount}-{txReceipt.TransactionHash}");
                return txReceipt.TransactionHash;
            }
            catch (Exception ex)
            {
                _logger.LogError($"MakeCBDCTransfer-EX-{ex}");
            }
            return String.Empty;
        }

    }
}

