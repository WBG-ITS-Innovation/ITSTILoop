using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using Cbdchubcontract.Contracts.CbTransferContract.ContractDefinition;

namespace Cbdchubcontract.Contracts.CbTransferContract
{
    public partial class CbTransferContractService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, CbTransferContractDeployment cbTransferContractDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<CbTransferContractDeployment>().SendRequestAndWaitForReceiptAsync(cbTransferContractDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, CbTransferContractDeployment cbTransferContractDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<CbTransferContractDeployment>().SendRequestAsync(cbTransferContractDeployment);
        }

        public static async Task<CbTransferContractService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, CbTransferContractDeployment cbTransferContractDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, cbTransferContractDeployment, cancellationTokenSource);
            return new CbTransferContractService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public CbTransferContractService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> GetCBDCTokenQueryAsync(GetCBDCTokenFunction getCBDCTokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCBDCTokenFunction, string>(getCBDCTokenFunction, blockParameter);
        }

        
        public Task<string> GetCBDCTokenQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCBDCTokenFunction, string>(null, blockParameter);
        }

        public Task<GetTransferOutputDTO> GetTransferQueryAsync(GetTransferFunction getTransferFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetTransferFunction, GetTransferOutputDTO>(getTransferFunction, blockParameter);
        }

        public Task<GetTransferOutputDTO> GetTransferQueryAsync(string from, string to, BigInteger amount, BigInteger timestamp, BlockParameter blockParameter = null)
        {
            var getTransferFunction = new GetTransferFunction();
                getTransferFunction.From = from;
                getTransferFunction.To = to;
                getTransferFunction.Amount = amount;
                getTransferFunction.Timestamp = timestamp;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetTransferFunction, GetTransferOutputDTO>(getTransferFunction, blockParameter);
        }

        public Task<string> MakeTransferRequestAsync(MakeTransferFunction makeTransferFunction)
        {
             return ContractHandler.SendRequestAsync(makeTransferFunction);
        }

        public Task<TransactionReceipt> MakeTransferRequestAndWaitForReceiptAsync(MakeTransferFunction makeTransferFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(makeTransferFunction, cancellationToken);
        }

        public Task<string> MakeTransferRequestAsync(string from, string to, BigInteger amount, string destination, byte toType)
        {
            var makeTransferFunction = new MakeTransferFunction();
                makeTransferFunction.From = from;
                makeTransferFunction.To = to;
                makeTransferFunction.Amount = amount;
                makeTransferFunction.Destination = destination;
                makeTransferFunction.ToType = toType;                
            
             return ContractHandler.SendRequestAsync(makeTransferFunction);
        }

        public Task<TransactionReceipt> MakeTransferRequestAndWaitForReceiptAsync(string from, string to, BigInteger amount, string destination, byte toType, CancellationTokenSource cancellationToken = null)
        {
            var makeTransferFunction = new MakeTransferFunction();
                makeTransferFunction.From = from;
                makeTransferFunction.To = to;
                makeTransferFunction.Amount = amount;
                makeTransferFunction.Destination = destination;
                makeTransferFunction.ToType = toType;                
            return ContractHandler.SendRequestAndWaitForReceiptAsync(makeTransferFunction, cancellationToken);
        }

        public Task<string> RegisterPSPRequestAsync(RegisterPSPFunction registerPSPFunction)
        {
             return ContractHandler.SendRequestAsync(registerPSPFunction);
        }

        public Task<TransactionReceipt> RegisterPSPRequestAndWaitForReceiptAsync(RegisterPSPFunction registerPSPFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerPSPFunction, cancellationToken);
        }

        public Task<string> RegisterPSPRequestAsync(string pspAddress, string pspName)
        {
            var registerPSPFunction = new RegisterPSPFunction();
                registerPSPFunction.PspAddress = pspAddress;
                registerPSPFunction.PspName = pspName;
            
             return ContractHandler.SendRequestAsync(registerPSPFunction);
        }

        public Task<TransactionReceipt> RegisterPSPRequestAndWaitForReceiptAsync(string pspAddress, string pspName, CancellationTokenSource cancellationToken = null)
        {
            var registerPSPFunction = new RegisterPSPFunction();
                registerPSPFunction.PspAddress = pspAddress;
                registerPSPFunction.PspName = pspName;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerPSPFunction, cancellationToken);
        }

        public Task<string> SetCBDCTokenRequestAsync(SetCBDCTokenFunction setCBDCTokenFunction)
        {
             return ContractHandler.SendRequestAsync(setCBDCTokenFunction);
        }

        public Task<TransactionReceipt> SetCBDCTokenRequestAndWaitForReceiptAsync(SetCBDCTokenFunction setCBDCTokenFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setCBDCTokenFunction, cancellationToken);
        }

        public Task<string> SetCBDCTokenRequestAsync(string tokenAddress)
        {
            var setCBDCTokenFunction = new SetCBDCTokenFunction();
                setCBDCTokenFunction.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAsync(setCBDCTokenFunction);
        }

        public Task<TransactionReceipt> SetCBDCTokenRequestAndWaitForReceiptAsync(string tokenAddress, CancellationTokenSource cancellationToken = null)
        {
            var setCBDCTokenFunction = new SetCBDCTokenFunction();
                setCBDCTokenFunction.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setCBDCTokenFunction, cancellationToken);
        }

        public Task<string> SetOwnerRequestAsync(SetOwnerFunction setOwnerFunction)
        {
             return ContractHandler.SendRequestAsync(setOwnerFunction);
        }

        public Task<TransactionReceipt> SetOwnerRequestAndWaitForReceiptAsync(SetOwnerFunction setOwnerFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setOwnerFunction, cancellationToken);
        }

        public Task<string> SetOwnerRequestAsync(string ownerAddress)
        {
            var setOwnerFunction = new SetOwnerFunction();
                setOwnerFunction.OwnerAddress = ownerAddress;
            
             return ContractHandler.SendRequestAsync(setOwnerFunction);
        }

        public Task<TransactionReceipt> SetOwnerRequestAndWaitForReceiptAsync(string ownerAddress, CancellationTokenSource cancellationToken = null)
        {
            var setOwnerFunction = new SetOwnerFunction();
                setOwnerFunction.OwnerAddress = ownerAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setOwnerFunction, cancellationToken);
        }

        public Task<string> TransferCompleteRequestAsync(TransferCompleteFunction transferCompleteFunction)
        {
             return ContractHandler.SendRequestAsync(transferCompleteFunction);
        }

        public Task<TransactionReceipt> TransferCompleteRequestAndWaitForReceiptAsync(TransferCompleteFunction transferCompleteFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferCompleteFunction, cancellationToken);
        }

        public Task<string> TransferCompleteRequestAsync(byte[] transferHash)
        {
            var transferCompleteFunction = new TransferCompleteFunction();
                transferCompleteFunction.TransferHash = transferHash;
            
             return ContractHandler.SendRequestAsync(transferCompleteFunction);
        }

        public Task<TransactionReceipt> TransferCompleteRequestAndWaitForReceiptAsync(byte[] transferHash, CancellationTokenSource cancellationToken = null)
        {
            var transferCompleteFunction = new TransferCompleteFunction();
                transferCompleteFunction.TransferHash = transferHash;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferCompleteFunction, cancellationToken);
        }

        public Task<string> TransferFailRequestAsync(TransferFailFunction transferFailFunction)
        {
             return ContractHandler.SendRequestAsync(transferFailFunction);
        }

        public Task<TransactionReceipt> TransferFailRequestAndWaitForReceiptAsync(TransferFailFunction transferFailFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFailFunction, cancellationToken);
        }

        public Task<string> TransferFailRequestAsync(byte[] transferHash)
        {
            var transferFailFunction = new TransferFailFunction();
                transferFailFunction.TransferHash = transferHash;
            
             return ContractHandler.SendRequestAsync(transferFailFunction);
        }

        public Task<TransactionReceipt> TransferFailRequestAndWaitForReceiptAsync(byte[] transferHash, CancellationTokenSource cancellationToken = null)
        {
            var transferFailFunction = new TransferFailFunction();
                transferFailFunction.TransferHash = transferHash;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFailFunction, cancellationToken);
        }
    }
}
