using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Cbdchubcontract.Contracts.CbTransferContract.ContractDefinition
{
    public partial class TransferOrder : TransferOrderBase { }

    public class TransferOrderBase 
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("string", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "amount", 3)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("string", "destination", 4)]
        public virtual string Destination { get; set; }
        [Parameter("uint8", "toType", 5)]
        public virtual byte ToType { get; set; }
        [Parameter("uint256", "timestamp", 6)]
        public virtual BigInteger Timestamp { get; set; }
        [Parameter("uint8", "status", 7)]
        public virtual byte Status { get; set; }
        [Parameter("bytes32", "transferHash", 8)]
        public virtual byte[] TransferHash { get; set; }
    }
}
