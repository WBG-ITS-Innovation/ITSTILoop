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
using CBDCFPShubContract.Contracts.HubContract.ContractDefinition;

namespace CBDCFPShubContract.Contracts.HubContract
{
    public partial class HubContractService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, HubContractDeployment hubContractDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<HubContractDeployment>().SendRequestAndWaitForReceiptAsync(hubContractDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, HubContractDeployment hubContractDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<HubContractDeployment>().SendRequestAsync(hubContractDeployment);
        }

        public static async Task<HubContractService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, HubContractDeployment hubContractDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, hubContractDeployment, cancellationTokenSource);
            return new HubContractService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public HubContractService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> DebtFSPRequestAsync(DebtFSPFunction debtFSPFunction)
        {
             return ContractHandler.SendRequestAsync(debtFSPFunction);
        }

        public Task<TransactionReceipt> DebtFSPRequestAndWaitForReceiptAsync(DebtFSPFunction debtFSPFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(debtFSPFunction, cancellationToken);
        }

        public Task<string> DebtFSPRequestAsync(string payer, BigInteger numTokens)
        {
            var debtFSPFunction = new DebtFSPFunction();
                debtFSPFunction.Payer = payer;
                debtFSPFunction.NumTokens = numTokens;
            
             return ContractHandler.SendRequestAsync(debtFSPFunction);
        }

        public Task<TransactionReceipt> DebtFSPRequestAndWaitForReceiptAsync(string payer, BigInteger numTokens, CancellationTokenSource cancellationToken = null)
        {
            var debtFSPFunction = new DebtFSPFunction();
                debtFSPFunction.Payer = payer;
                debtFSPFunction.NumTokens = numTokens;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(debtFSPFunction, cancellationToken);
        }

        public Task<string> SetOwnerRequestAsync(SetOwnerFunction setOwnerFunction)
        {
             return ContractHandler.SendRequestAsync(setOwnerFunction);
        }

        public Task<TransactionReceipt> SetOwnerRequestAndWaitForReceiptAsync(SetOwnerFunction setOwnerFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setOwnerFunction, cancellationToken);
        }

        public Task<string> SetOwnerRequestAsync(string owner)
        {
            var setOwnerFunction = new SetOwnerFunction();
                setOwnerFunction.Owner = owner;
            
             return ContractHandler.SendRequestAsync(setOwnerFunction);
        }

        public Task<TransactionReceipt> SetOwnerRequestAndWaitForReceiptAsync(string owner, CancellationTokenSource cancellationToken = null)
        {
            var setOwnerFunction = new SetOwnerFunction();
                setOwnerFunction.Owner = owner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setOwnerFunction, cancellationToken);
        }

        public Task<string> SettleAccountRequestAsync(SettleAccountFunction settleAccountFunction)
        {
             return ContractHandler.SendRequestAsync(settleAccountFunction);
        }

        public Task<TransactionReceipt> SettleAccountRequestAndWaitForReceiptAsync(SettleAccountFunction settleAccountFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(settleAccountFunction, cancellationToken);
        }

        public Task<string> SettleAccountRequestAsync(string payer, string payee, BigInteger numTokens)
        {
            var settleAccountFunction = new SettleAccountFunction();
                settleAccountFunction.Payer = payer;
                settleAccountFunction.Payee = payee;
                settleAccountFunction.NumTokens = numTokens;
            
             return ContractHandler.SendRequestAsync(settleAccountFunction);
        }

        public Task<TransactionReceipt> SettleAccountRequestAndWaitForReceiptAsync(string payer, string payee, BigInteger numTokens, CancellationTokenSource cancellationToken = null)
        {
            var settleAccountFunction = new SettleAccountFunction();
                settleAccountFunction.Payer = payer;
                settleAccountFunction.Payee = payee;
                settleAccountFunction.NumTokens = numTokens;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(settleAccountFunction, cancellationToken);
        }

        public Task<string> FundFSPRequestAsync(FundFSPFunction fundFSPFunction)
        {
             return ContractHandler.SendRequestAsync(fundFSPFunction);
        }

        public Task<TransactionReceipt> FundFSPRequestAndWaitForReceiptAsync(FundFSPFunction fundFSPFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(fundFSPFunction, cancellationToken);
        }

        public Task<string> FundFSPRequestAsync(BigInteger numTokens)
        {
            var fundFSPFunction = new FundFSPFunction();
                fundFSPFunction.NumTokens = numTokens;
            
             return ContractHandler.SendRequestAsync(fundFSPFunction);
        }

        public Task<TransactionReceipt> FundFSPRequestAndWaitForReceiptAsync(BigInteger numTokens, CancellationTokenSource cancellationToken = null)
        {
            var fundFSPFunction = new FundFSPFunction();
                fundFSPFunction.NumTokens = numTokens;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(fundFSPFunction, cancellationToken);
        }

        public Task<BigInteger> ContractBalanceQueryAsync(ContractBalanceFunction contractBalanceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ContractBalanceFunction, BigInteger>(contractBalanceFunction, blockParameter);
        }

        
        public Task<BigInteger> ContractBalanceQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ContractBalanceFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> SetCBDCTokenRequestAsync(SetCBDCTokenFunction setCBDCTokenFunction)
        {
             return ContractHandler.SendRequestAsync(setCBDCTokenFunction);
        }

        public Task<TransactionReceipt> SetCBDCTokenRequestAndWaitForReceiptAsync(SetCBDCTokenFunction setCBDCTokenFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setCBDCTokenFunction, cancellationToken);
        }

        public Task<string> SetCBDCTokenRequestAsync(string tokenaddr)
        {
            var setCBDCTokenFunction = new SetCBDCTokenFunction();
                setCBDCTokenFunction.Tokenaddr = tokenaddr;
            
             return ContractHandler.SendRequestAsync(setCBDCTokenFunction);
        }

        public Task<TransactionReceipt> SetCBDCTokenRequestAndWaitForReceiptAsync(string tokenaddr, CancellationTokenSource cancellationToken = null)
        {
            var setCBDCTokenFunction = new SetCBDCTokenFunction();
                setCBDCTokenFunction.Tokenaddr = tokenaddr;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setCBDCTokenFunction, cancellationToken);
        }

        public Task<BigInteger> SettlementBalanceOfQueryAsync(SettlementBalanceOfFunction settlementBalanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SettlementBalanceOfFunction, BigInteger>(settlementBalanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> SettlementBalanceOfQueryAsync(string tokenOwner, BlockParameter blockParameter = null)
        {
            var settlementBalanceOfFunction = new SettlementBalanceOfFunction();
                settlementBalanceOfFunction.TokenOwner = tokenOwner;
            
            return ContractHandler.QueryAsync<SettlementBalanceOfFunction, BigInteger>(settlementBalanceOfFunction, blockParameter);
        }

        public Task<string> RegisterFSPRequestAsync(RegisterFSPFunction registerFSPFunction)
        {
             return ContractHandler.SendRequestAsync(registerFSPFunction);
        }

        public Task<TransactionReceipt> RegisterFSPRequestAndWaitForReceiptAsync(RegisterFSPFunction registerFSPFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerFSPFunction, cancellationToken);
        }

        public Task<string> RegisterFSPRequestAsync(string addr, string name, string id)
        {
            var registerFSPFunction = new RegisterFSPFunction();
                registerFSPFunction.Addr = addr;
                registerFSPFunction.Name = name;
                registerFSPFunction.Id = id;
            
             return ContractHandler.SendRequestAsync(registerFSPFunction);
        }

        public Task<TransactionReceipt> RegisterFSPRequestAndWaitForReceiptAsync(string addr, string name, string id, CancellationTokenSource cancellationToken = null)
        {
            var registerFSPFunction = new RegisterFSPFunction();
                registerFSPFunction.Addr = addr;
                registerFSPFunction.Name = name;
                registerFSPFunction.Id = id;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerFSPFunction, cancellationToken);
        }

        public Task<string> PayoutFSPRequestAsync(PayoutFSPFunction payoutFSPFunction)
        {
             return ContractHandler.SendRequestAsync(payoutFSPFunction);
        }

        public Task<TransactionReceipt> PayoutFSPRequestAndWaitForReceiptAsync(PayoutFSPFunction payoutFSPFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(payoutFSPFunction, cancellationToken);
        }

        public Task<string> PayoutFSPRequestAsync(string payee, BigInteger numTokens)
        {
            var payoutFSPFunction = new PayoutFSPFunction();
                payoutFSPFunction.Payee = payee;
                payoutFSPFunction.NumTokens = numTokens;
            
             return ContractHandler.SendRequestAsync(payoutFSPFunction);
        }

        public Task<TransactionReceipt> PayoutFSPRequestAndWaitForReceiptAsync(string payee, BigInteger numTokens, CancellationTokenSource cancellationToken = null)
        {
            var payoutFSPFunction = new PayoutFSPFunction();
                payoutFSPFunction.Payee = payee;
                payoutFSPFunction.NumTokens = numTokens;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(payoutFSPFunction, cancellationToken);
        }
    }
}
