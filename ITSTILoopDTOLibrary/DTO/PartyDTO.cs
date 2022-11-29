namespace ITSTILoopLibrary.DTO
{
    public enum PartyIdTypes { MSISDN, Email };
    public class PartyDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string CbdcAddress { get; set; } = string.Empty;
        public string PSPName { get; set; } = string.Empty;
        public string HubName { get; set; } = string.Empty;
        public int ParticipantId { get; set; }
        public PartyIdentifierDTO PartyIdentifier { get; set; } = new PartyIdentifierDTO();

    }

    public class PartyIdentifierDTO
    {
        public string Identifier { get; set; } = string.Empty;
        public PartyIdTypes PartyIdentifierType { get; set; }
    }

}