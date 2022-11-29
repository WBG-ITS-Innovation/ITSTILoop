using System;
namespace ITSTILoopCBDCAdapter.Configuration
{
	public class TransferContractConfig
	{
		public TransferContractConfig()
		{
		}
        public string ContractAddress { get; set; } = String.Empty;
        public string ContractOwnerKey { get; set; } = String.Empty;
        public string ContractTransactionHash { get; set; } = String.Empty;
        public string RpcEndpoint { get; set; } = String.Empty;
        public int NetworkId { get; set; } = 0;
    }
}

