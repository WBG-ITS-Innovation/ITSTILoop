using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITSTILoopDTOLibrary
{
    public class TransferAcceptRejectDTO
    {
        public bool AcceptTransfer { get; set; }
    }


    public enum TransferStates { WaitingForPartyAcceptance, WaitingForQuoteAcceptance, Completed}
    public class TransferRequestDTO
    {
        public Guid HomeTransactionId { get; set; }
        public QueryPartyDTO From { get; set; }
        public QueryPartyDTO To { get; set; }
        public CurrencyCodes Currency { get; set; }
        public Decimal Amount { get; set; }
        public string Note { get; set; }
    }

    public class TransferRequestResponseDTO
    {
        public Guid HomeTransactionId { get; set; }
        public Guid TransferId { get; set; }
        public QueryPartyDTO From { get; set; }
        public PartyDTO To { get; set; }
        public CurrencyCodes Currency { get; set; }
        public Decimal Amount { get; set; }
        public string Note { get; set; }
        public TransferStates CurrentState { get; set; }
        public DateTime InitiatedTimestamp { get; set; }
    }

    public class TransferFulfilmentDTO
    {
        public DateTime CompletedTimestamp { get; set; }
        public string Fullfilment { get; set; }
    }
    
    public class TransferRequestCompleteDTO : TransferRequestResponseDTO
    {
        public TransferFulfilmentDTO Fulfil { get; set; }
    }
}
