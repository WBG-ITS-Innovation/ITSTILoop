
namespace ITSTILoopDTOLibrary
{
    public enum PartyIdTypes { MSISDN, Email };
    public class PartyDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PartyIdentifier { get; set; }
        public string RegisteredParticipantName { get; set; }
        public PartyIdTypes PartyIdentifierType { get; set; }
    }

    public class QueryPartyDTO
    {
        public string PartyIdentifier { get; set; }
        public PartyIdTypes PartyIdentifierType { get; set; }
    }

    public class RegisterPartyDTO
    {        
        public string PartyIdentifier { get; set; }
        public PartyIdTypes PartyIdentifierType { get; set; }
    }
}
