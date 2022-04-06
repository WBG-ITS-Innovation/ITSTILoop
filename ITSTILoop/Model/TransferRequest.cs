using ITSTILoopDTOLibrary;

namespace ITSTILoop.Model
{
    public class TransferRequest
    {
        public int TransferRequestId { get; set; }
        public Guid HomeTransactionId { get; set; }
        public Guid TransferId { get; set; }
        public string FromPartyIdentifier { get; set; }
        public PartyIdTypes FromPartyIdentifierType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ToPartyIdentifier { get; set; }
        public string ToRegisteredParticipantName { get; set; }
        public PartyIdTypes ToPartyIdentifierType { get; set; }
        public CurrencyCodes Currency { get; set; }
        public Decimal Amount { get; set; }
        public string Note { get; set; }
        public TransferStates CurrentState { get; set; }
        public DateTime InitiatedTimestamp { get; set; }

    }
}
