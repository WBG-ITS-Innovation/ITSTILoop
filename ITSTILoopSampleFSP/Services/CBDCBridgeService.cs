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

namespace ITSTILoopSampleFSP.Services
{
    public enum TransferDestinationType { Hub, Bank};

    public class CBDCBridgeService
    {
        private readonly ILogger<CBDCBridgeService> _logger;

        private readonly Web3 _web3;
        private readonly CbTransferContractService _cbTransferContractService;

        public CBDCBridgeService(ILogger<CBDCBridgeService> logger)
        {
            _logger = logger;

            IClient client = new RpcClient(new Uri(EnvironmentVariables.GetEnvironmentVariable(EnvironmentVariableNames.CBDC_RPC_ENDPOINT, EnvironmentVariableDefaultValues.CBDC_RPC_ENDPOINT_DEFAULT_VALUE)));
            _web3 = new Web3(new Account(EnvironmentVariables.GetEnvironmentVariable(EnvironmentVariableNames.CBDC_TRANSFER_CONTRACT_OWNER_KEY,EnvironmentVariableDefaultValues.CBDC_TRANSFER_CONTRACT_OWNER_KEY_DEFAULT_VALUE),
                Convert.ToInt32(EnvironmentVariables.GetEnvironmentVariable(EnvironmentVariableNames.CBDC_NETWORK_ID, EnvironmentVariableDefaultValues.CBDC_NETWORK_ID_DEFAULT_VALUE))), client);
            _web3.TransactionManager.UseLegacyAsDefault = true;
            _cbTransferContractService = new CbTransferContractService(_web3, EnvironmentVariables.GetEnvironmentVariable(EnvironmentVariableNames.CBDC_TRANSFER_CONTRACT_ADDRESS, EnvironmentVariableDefaultValues.CBDC_TRANSFER_CONTRACT_ADDRESS_DEFAULT_VALUE));
        }

        public async Task<string> MakeTransfer(string from, string to, int amount, string destination, TransferDestinationType destinationType = TransferDestinationType.Hub )
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

    }
}

