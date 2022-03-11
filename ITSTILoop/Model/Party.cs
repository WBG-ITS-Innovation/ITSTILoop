using ITSTILoopDTOLibrary;

namespace ITSTILoop.Model
{    
    public class Party
    {
        public int PartyId { get; set; }
        public string RegisteredParticipantName { get; set; }
        public string PartyIdentifier { get; set; }
        public PartyIdTypes PartyIdentifierType { get; set; }
    }
}
