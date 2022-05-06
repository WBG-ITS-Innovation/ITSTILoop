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
        public static string BYTECODE = "608060405234801561001057600080fd5b50604051602080610c2383398101604052516000600481905560018054600160a060020a03909316600160a060020a0319938416179055805490911633179055610bc48061005f6000396000f3006080604052600436106100985763ffffffff7c01000000000000000000000000000000000000000000000000000000006000350416630a4a7948811461009d57806313af4035146100d55780636a8f9bbd146100f85780636f38e2d0146101225780638b7afe2e1461013a578063a0e04e4a14610161578063a29be8a714610182578063bf7fb9db146101a3578063f059c50014610248575b600080fd5b3480156100a957600080fd5b506100c1600160a060020a036004351660243561026c565b604080519115158252519081900360200190f35b3480156100e157600080fd5b506100f6600160a060020a03600435166103bc565b005b34801561010457600080fd5b506100c1600160a060020a0360043581169060243516604435610402565b34801561012e57600080fd5b506100c1600435610586565b34801561014657600080fd5b5061014f61073e565b60408051918252519081900360200190f35b34801561016d57600080fd5b506100f6600160a060020a0360043516610745565b34801561018e57600080fd5b5061014f600160a060020a036004351661078b565b3480156101af57600080fd5b5060408051602060046024803582810135601f81018590048502860185019096528585526100f6958335600160a060020a031695369560449491939091019190819084018382808284375050604080516020601f89358b018035918201839004830284018301909452808352979a9998810197919650918201945092508291508401838280828437509497506107a69650505050505050565b34801561025457600080fd5b506100c1600160a060020a0360043516602435610929565b60008054600160a060020a0316331461028457600080fd5b600160a060020a0383166000908152600360205260409020548211156102a957600080fd5b600160a060020a0383166000908152600360205260409020546102d2908363ffffffff610ad816565b600160a060020a0384166000818152600360209081526040808320949094553382526002808252918490208451918201879052848252600190810180549182161561010002600019019091169290920493810184905291927f3ef2d3d9e0c041bdc7a21d6c8801cb0efd5f2983413cf7b9114526cf8687760292869181906060820190859080156103a45780601f10610379576101008083540402835291602001916103a4565b820191906000526020600020905b81548152906001019060200180831161038757829003601f168201915b5050935050505060405180910390a250600192915050565b600054600160a060020a031633146103d357600080fd5b6000805473ffffffffffffffffffffffffffffffffffffffff1916600160a060020a0392909216919091179055565b600080548190600160a060020a0316331461041c57600080fd5b600160a060020a03851660009081526003602052604090205483111561044157600080fd5b50600154604080517fa9059cbb000000000000000000000000000000000000000000000000000000008152600160a060020a0386811660048301526024820186905291519190921691829163a9059cbb916044808201926020929091908290030181600087803b1580156104b457600080fd5b505af11580156104c8573d6000803e3d6000fd5b505050506040513d60208110156104de57600080fd5b50511561057957600160a060020a03851660009081526003602052604090205461050e908463ffffffff610ad816565b600160a060020a0380871660008181526003602090815260409182902094909455600480548890039055805187815290519288169391927fca2ce982d63c71a419517d389df253be4b0d6f4da85fe1491e49608b139ee0be929181900390910190a36001915061057e565b600091505b509392505050565b600154604080517f23b872dd000000000000000000000000000000000000000000000000000000008152336004820152306024820152604481018490529051600092600160a060020a03169182916323b872dd9160648082019260209290919082900301818887803b1580156105fb57600080fd5b505af115801561060f573d6000803e3d6000fd5b505050506040513d602081101561062557600080fd5b505115610733573360009081526003602052604090205461064c908463ffffffff610aea16565b3360008181526003602090815260408083209490945560048054880190556002808252918490208451888152918201858152600191820180549283161561010002600019019092169390930494820185905292937f6e045e3d8c697ffb911484d9d2a107c13e887d27fbc9e643086cf3b4ff71212593889390929160608301908490801561071b5780601f106106f05761010080835404028352916020019161071b565b820191906000526020600020905b8154815290600101906020018083116106fe57829003601f168201915b5050935050505060405180910390a260019150610738565b600091505b50919050565b6004545b90565b600054600160a060020a0316331461075c57600080fd5b6001805473ffffffffffffffffffffffffffffffffffffffff1916600160a060020a0392909216919091179055565b600160a060020a031660009081526003602052604090205490565b600054600160a060020a031633146107bd57600080fd5b6040805180820182528381526020808201849052600160a060020a0386166000908152600282529290922081518051929391926107fd9284920190610b00565b5060208281015180516108169260018501920190610b00565b5090505082600160a060020a03167f9a7974637569d172a99b047b5b3a77a894868f97984379bdee73f79bc7601b8d8383604051808060200180602001838103835285818151815260200191508051906020019080838360005b83811015610888578181015183820152602001610870565b50505050905090810190601f1680156108b55780820380516001836020036101000a031916815260200191505b50838103825284518152845160209182019186019080838360005b838110156108e85781810151838201526020016108d0565b50505050905090810190601f1680156109155780820380516001836020036101000a031916815260200191505b5094505050505060405180910390a2505050565b600080548190600160a060020a0316331461094357600080fd5b50600154604080517fa9059cbb000000000000000000000000000000000000000000000000000000008152600160a060020a0386811660048301526024820186905291519190921691829163a9059cbb916044808201926020929091908290030181600087803b1580156109b657600080fd5b505af11580156109ca573d6000803e3d6000fd5b505050506040513d60208110156109e057600080fd5b505115610acc576004805484900390553360009081526002602081815260409283902083519182018790528382526001908101805491821615610100026000190190911692909204928101839052600160a060020a038716927f6906d0fda550703e8407966ff9626e12258614665c88059f7a3b6f6db67b4f17929187918190606082019085908015610ab45780601f10610a8957610100808354040283529160200191610ab4565b820191906000526020600020905b815481529060010190602001808311610a9757829003601f168201915b5050935050505060405180910390a260019150610ad1565b600091505b5092915050565b600082821115610ae457fe5b50900390565b600082820183811015610af957fe5b9392505050565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f10610b4157805160ff1916838001178555610b6e565b82800160010185558215610b6e579182015b82811115610b6e578251825591602001919060010190610b53565b50610b7a929150610b7e565b5090565b61074291905b80821115610b7a5760008155600101610b845600a165627a7a723058201aa151e84f19fc828ff8ae6b509d12221683204bac4965557d77055190e2009e0029";
        public HubContractDeploymentBase() : base(BYTECODE) { }
        public HubContractDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("address", "_CBDCToken", 1)]
        public virtual string CBDCToken { get; set; }
    }

    public partial class DebtFSPFunction : DebtFSPFunctionBase { }

    [Function("debtFSP", "bool")]
    public class DebtFSPFunctionBase : FunctionMessage
    {
        [Parameter("address", "payer", 1)]
        public virtual string Payer { get; set; }
        [Parameter("uint256", "numTokens", 2)]
        public virtual BigInteger NumTokens { get; set; }
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

    public partial class SettlementBalanceOfFunction : SettlementBalanceOfFunctionBase { }

    [Function("settlementBalanceOf", "uint256")]
    public class SettlementBalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "tokenOwner", 1)]
        public virtual string TokenOwner { get; set; }
    }

    public partial class RegisterFSPFunction : RegisterFSPFunctionBase { }

    [Function("registerFSP")]
    public class RegisterFSPFunctionBase : FunctionMessage
    {
        [Parameter("address", "_addr", 1)]
        public virtual string Addr { get; set; }
        [Parameter("string", "_name", 2)]
        public virtual string Name { get; set; }
        [Parameter("string", "_id", 3)]
        public virtual string Id { get; set; }
    }

    public partial class PayoutFSPFunction : PayoutFSPFunctionBase { }

    [Function("payoutFSP", "bool")]
    public class PayoutFSPFunctionBase : FunctionMessage
    {
        [Parameter("address", "payee", 1)]
        public virtual string Payee { get; set; }
        [Parameter("uint256", "numTokens", 2)]
        public virtual BigInteger NumTokens { get; set; }
    }

    public partial class AccountFundedEventDTO : AccountFundedEventDTOBase { }

    [Event("AccountFunded")]
    public class AccountFundedEventDTOBase : IEventDTO
    {
        [Parameter("address", "FSP", 1, true )]
        public virtual string Fsp { get; set; }
        [Parameter("uint256", "tokens", 2, false )]
        public virtual BigInteger Tokens { get; set; }
        [Parameter("string", "id", 3, false )]
        public virtual string Id { get; set; }
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

    public partial class FSPpayoutEventDTO : FSPpayoutEventDTOBase { }

    [Event("FSPpayout")]
    public class FSPpayoutEventDTOBase : IEventDTO
    {
        [Parameter("address", "addr", 1, true )]
        public virtual string Addr { get; set; }
        [Parameter("string", "id", 2, false )]
        public virtual string Id { get; set; }
        [Parameter("uint256", "tokens", 3, false )]
        public virtual BigInteger Tokens { get; set; }
    }

    public partial class FSPdebtEventDTO : FSPdebtEventDTOBase { }

    [Event("FSPdebt")]
    public class FSPdebtEventDTOBase : IEventDTO
    {
        [Parameter("address", "addr", 1, true )]
        public virtual string Addr { get; set; }
        [Parameter("string", "id", 2, false )]
        public virtual string Id { get; set; }
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
        [Parameter("string", "id", 3, false )]
        public virtual string Id { get; set; }
    }









    public partial class ContractBalanceOutputDTO : ContractBalanceOutputDTOBase { }

    [FunctionOutput]
    public class ContractBalanceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class SettlementBalanceOfOutputDTO : SettlementBalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class SettlementBalanceOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }




}
