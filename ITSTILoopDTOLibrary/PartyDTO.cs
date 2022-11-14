
namespace ITSTILoopDTOLibrary
{
    public enum PartyIdTypes { MSISDN, Email };
    public class PartyDTO
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;        
        public int ParticipantId { get; set; }
        public PartyIdentifierDTO PartyIdentifier { get; set; } = new PartyIdentifierDTO();
        
    }

    public class PartyIdentifierDTO
    {
        public string Identifier { get; set; } = String.Empty;
        public PartyIdTypes PartyIdentifierType { get; set; }
    }

    public class GlobalPartyIdentifierDTO
    {
        public PartyIdentifierDTO IdentifierDto { get; set; } = new PartyIdentifierDTO();
        public string FSPName { get; set; } = string.Empty;
        public string HubName { get; set; } = string.Empty;
    }
}