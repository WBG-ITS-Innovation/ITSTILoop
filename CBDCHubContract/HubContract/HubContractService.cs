using System.Numerics;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.ContractHandlers;
using CBDCHubContract.Contracts.HubContract.ContractDefinition;

namespace CBDCHubContract.Contracts.HubContract
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

        public Task<string> GetCBDCTokenQueryAsync(GetCBDCTokenFunction getCBDCTokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCBDCTokenFunction, string>(getCBDCTokenFunction, blockParameter);
        }

        
        public Task<string> GetCBDCTokenQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetCBDCTokenFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> GetContractBalanceQueryAsync(GetContractBalanceFunction getContractBalanceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetContractBalanceFunction, BigInteger>(getContractBalanceFunction, blockParameter);
        }

        
        public Task<BigInteger> GetContractBalanceQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetContractBalanceFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> GetFSPBalanceQueryAsync(GetFSPBalanceFunction getFSPBalanceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetFSPBalanceFunction, BigInteger>(getFSPBalanceFunction, blockParameter);
        }

        
        public Task<BigInteger> GetFSPBalanceQueryAsync(string fspAddress, BlockParameter blockParameter = null)
        {
            var getFSPBalanceFunction = new GetFSPBalanceFunction();
                getFSPBalanceFunction.FspAddress = fspAddress;
            
            return ContractHandler.QueryAsync<GetFSPBalanceFunction, BigInteger>(getFSPBalanceFunction, blockParameter);
        }

        public Task<string> MultilateralSettlementRequestAsync(MultilateralSettlementFunction multilateralSettlementFunction)
        {
             return ContractHandler.SendRequestAsync(multilateralSettlementFunction);
        }

        public Task<TransactionReceipt> MultilateralSettlementRequestAndWaitForReceiptAsync(MultilateralSettlementFunction multilateralSettlementFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(multilateralSettlementFunction, cancellationToken);
        }

        public Task<string> MultilateralSettlementRequestAsync(BigInteger settlementId, List<string> accounts, List<BigInteger> positions)
        {
            var multilateralSettlementFunction = new MultilateralSettlementFunction();
                multilateralSettlementFunction.SettlementId = settlementId;
                multilateralSettlementFunction.Accounts = accounts;
                multilateralSettlementFunction.Positions = positions;
            
             return ContractHandler.SendRequestAsync(multilateralSettlementFunction);
        }

        public Task<TransactionReceipt> MultilateralSettlementRequestAndWaitForReceiptAsync(BigInteger settlementId, List<string> accounts, List<BigInteger> positions, CancellationTokenSource cancellationToken = null)
        {
            var multilateralSettlementFunction = new MultilateralSettlementFunction();
                multilateralSettlementFunction.SettlementId = settlementId;
                multilateralSettlementFunction.Accounts = accounts;
                multilateralSettlementFunction.Positions = positions;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(multilateralSettlementFunction, cancellationToken);
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

        public Task<string> RegisterFSPRequestAsync(RegisterFSPFunction registerFSPFunction)
        {
             return ContractHandler.SendRequestAsync(registerFSPFunction);
        }

        public Task<TransactionReceipt> RegisterFSPRequestAndWaitForReceiptAsync(RegisterFSPFunction registerFSPFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerFSPFunction, cancellationToken);
        }

        public Task<string> RegisterFSPRequestAsync(string fspAddress, string fspName, string fspId)
        {
            var registerFSPFunction = new RegisterFSPFunction();
                registerFSPFunction.FspAddress = fspAddress;
                registerFSPFunction.FspName = fspName;
                registerFSPFunction.FspId = fspId;
            
             return ContractHandler.SendRequestAsync(registerFSPFunction);
        }

        public Task<TransactionReceipt> RegisterFSPRequestAndWaitForReceiptAsync(string fspAddress, string fspName, string fspId, CancellationTokenSource cancellationToken = null)
        {
            var registerFSPFunction = new RegisterFSPFunction();
                registerFSPFunction.FspAddress = fspAddress;
                registerFSPFunction.FspName = fspName;
                registerFSPFunction.FspId = fspId;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(registerFSPFunction, cancellationToken);
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
    }
}
