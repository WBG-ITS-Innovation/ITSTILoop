using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace CBDCFPShubContract.Contracts.HubContract.ContractDefinition
{


    public partial class HubContractDeployment : HubContractDeploymentBase
    {
        public HubContractDeployment() : base(BYTECODE) { }
        public HubContractDeployment(string byteCode) : base(byteCode) { }
    }

    public class HubContractDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b5060405160208061079183398101604052516000600481905560018054600160a060020a03909316600160a060020a03199384161790558054909116331790556107328061005f6000396000f3006080604052600436106100825763ffffffff7c010000000000000000000000000000000000000000000000000000000060003504166313af403581146100875780636a8f9bbd146100aa5780636f38e2d0146100e857806370a08231146101005780638b7afe2e14610133578063a0e04e4a14610148578063a1603c2514610169575b600080fd5b34801561009357600080fd5b506100a8600160a060020a03600435166101d2565b005b3480156100b657600080fd5b506100d4600160a060020a0360043581169060243516604435610218565b604080519115158252519081900360200190f35b3480156100f457600080fd5b506100d4600435610384565b34801561010c57600080fd5b50610121600160a060020a03600435166104c9565b60408051918252519081900360200190f35b34801561013f57600080fd5b506101216104e4565b34801561015457600080fd5b506100a8600160a060020a03600435166104eb565b34801561017557600080fd5b5060408051602060046024803582810135601f81018590048502860185019096528585526100a8958335600160a060020a031695369560449491939091019190819084018382808284375094975050933594506105319350505050565b600054600160a060020a031633146101e957600080fd5b6000805473ffffffffffffffffffffffffffffffffffffffff1916600160a060020a0392909216919091179055565b600160a060020a038316600090815260036020526040812054819083111561023f57600080fd5b50600154604080517fa9059cbb000000000000000000000000000000000000000000000000000000008152600160a060020a0386811660048301526024820186905291519190921691829163a9059cbb916044808201926020929091908290030181600087803b1580156102b257600080fd5b505af11580156102c6573d6000803e3d6000fd5b505050506040513d60208110156102dc57600080fd5b50511561037757600160a060020a03851660009081526003602052604090205461030c908463ffffffff61064616565b600160a060020a0380871660008181526003602090815260409182902094909455600480548890039055805187815290519288169391927fca2ce982d63c71a419517d389df253be4b0d6f4da85fe1491e49608b139ee0be929181900390910190a36001915061037c565b600091505b509392505050565b600080548190600160a060020a0316331461039e57600080fd5b50600154604080517f23b872dd000000000000000000000000000000000000000000000000000000008152336004820152306024820152604481018590529051600160a060020a039092169182916323b872dd9160648083019260209291908290030181600087803b15801561041357600080fd5b505af1158015610427573d6000803e3d6000fd5b505050506040513d602081101561043d57600080fd5b5051156104be5733600090815260036020526040902054610464908463ffffffff61065816565b336000818152600360209081526040918290209390935560048054870190558051868152905191927fbccbe05a3719eacef984f404dd2adff555adcc05fb72fb8b309bafbd462cd6f792918290030190a2600191506104c3565b600091505b50919050565b600160a060020a031660009081526003602052604090205490565b6004545b90565b600054600160a060020a0316331461050257600080fd5b6001805473ffffffffffffffffffffffffffffffffffffffff1916600160a060020a0392909216919091179055565b600054600160a060020a0316331461054857600080fd5b6040805180820182528381526020808201849052600160a060020a038616600090815260028252929092208151805192939192610588928492019061066e565b506020820151816001015590505082600160a060020a03167f3a01adc30ec5efaec7c6990aeab1dbdb0ad00147f1b0f6b0cb15980006f61c2083836040518080602001838152602001828103825284818151815260200191508051906020019080838360005b838110156106065781810151838201526020016105ee565b50505050905090810190601f1680156106335780820380516001836020036101000a031916815260200191505b50935050505060405180910390a2505050565b60008282111561065257fe5b50900390565b60008282018381101561066757fe5b9392505050565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f106106af57805160ff19168380011785556106dc565b828001600101855582156106dc579182015b828111156106dc5782518255916020019190600101906106c1565b506106e89291506106ec565b5090565b6104e891905b808211156106e857600081556001016106f25600a165627a7a72305820a14fbc53824d02c5ed102b9cc0c2551c6deef25041356fa49eba712123dd01720029";
        public HubContractDeploymentBase() : base(BYTECODE) { }
        public HubContractDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("address", "_CBDCToken", 1)]
        public virtual string CBDCToken { get; set; }
    }

    public partial class SetOwnerFunction : SetOwnerFunctionBase { }

    [Function("setOwner")]
    public class SetOwnerFunctionBase : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public virtual string Owner { get; set; }
    }

    public partial class SettleAccountFunction : SettleAccountFunctionBase { }

    [Function("settleAccount", "bool")]
    public class SettleAccountFunctionBase : FunctionMessage
    {
        [Parameter("address", "payer", 1)]
        public virtual string Payer { get; set; }
        [Parameter("address", "payee", 2)]
        public virtual string Payee { get; set; }
        [Parameter("uint256", "numTokens", 3)]
        public virtual BigInteger NumTokens { get; set; }
    }

    public partial class FundFSPFunction : FundFSPFunctionBase { }

    [Function("fundFSP", "bool")]
    public class FundFSPFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "numTokens", 1)]
        public virtual BigInteger NumTokens { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "tokenOwner", 1)]
        public virtual string TokenOwner { get; set; }
    }

    public partial class ContractBalanceFunction : ContractBalanceFunctionBase { }

    [Function("contractBalance", "uint256")]
    public class ContractBalanceFunctionBase : FunctionMessage
    {

    }

    public partial class SetCBDCTokenFunction : SetCBDCTokenFunctionBase { }

    [Function("setCBDCToken")]
    public class SetCBDCTokenFunctionBase : FunctionMessage
    {
        [Parameter("address", "_tokenaddr", 1)]
        public virtual string Tokenaddr { get; set; }
    }

    public partial class RegisterFSPFunction : RegisterFSPFunctionBase { }

    [Function("registerFSP")]
    public class RegisterFSPFunctionBase : FunctionMessage
    {
        [Parameter("address", "_addr", 1)]
        public virtual string Addr { get; set; }
        [Parameter("string", "_name", 2)]
        public virtual string Name { get; set; }
        [Parameter("uint256", "_id", 3)]
        public virtual BigInteger Id { get; set; }
    }

    public partial class AccountFundedEventDTO : AccountFundedEventDTOBase { }

    [Event("AccountFunded")]
    public class AccountFundedEventDTOBase : IEventDTO
    {
        [Parameter("address", "FSP", 1, true )]
        public virtual string Fsp { get; set; }
        [Parameter("uint256", "tokens", 2, false )]
        public virtual BigInteger Tokens { get; set; }
    }

    public partial class SettlementEventDTO : SettlementEventDTOBase { }

    [Event("Settlement")]
    public class SettlementEventDTOBase : IEventDTO
    {
        [Parameter("address", "payer", 1, true )]
        public virtual string Payer { get; set; }
        [Parameter("address", "payee", 2, true )]
        public virtual string Payee { get; set; }
        [Parameter("uint256", "tokens", 3, false )]
        public virtual BigInteger Tokens { get; set; }
    }

    public partial class FSPRegistrationEventDTO : FSPRegistrationEventDTOBase { }

    [Event("FSPRegistration")]
    public class FSPRegistrationEventDTOBase : IEventDTO
    {
        [Parameter("address", "addr", 1, true )]
        public virtual string Addr { get; set; }
        [Parameter("string", "name", 2, false )]
        public virtual string Name { get; set; }
        [Parameter("uint256", "id", 3, false )]
        public virtual BigInteger Id { get; set; }
    }







    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ContractBalanceOutputDTO : ContractBalanceOutputDTOBase { }

    [FunctionOutput]
    public class ContractBalanceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }




}
