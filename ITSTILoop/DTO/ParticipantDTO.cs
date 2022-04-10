using ITSTILoopDTOLibrary;

namespace ITSTILoop.DTO
{
    public class ParticipantDTO
    {
        public int ParticipantId { get; set; }
        public string Name { get; set; }        
        public Uri PartyLookupEndpoint { get; set; }
        public Uri ConfirmTransferEndpoint { get; set; }
    }

    public class RegisterParticipantDTO
    {
        public string Name { get; set; }
        public string ApiId { get; set; }
        public string ApiKey { get; set; }
        public Uri PartyLookupEndpoint { get; set; }
        public Uri ConfirmTransferEndpoint { get; set; }
    }

    public class FundParticipantDTO
    {
        public int ParticipantId { get; set; }
        public decimal Amount { get; set; }
        public CurrencyCodes Currency { get; set; }
    }
}
