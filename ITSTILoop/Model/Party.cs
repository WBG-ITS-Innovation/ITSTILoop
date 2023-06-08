using ITSTILoopLibrary.DTO;

namespace ITSTILoop.Model
{
    public class Party
    {
        public int PartyId { get; set; }
        public int ParticipantId { get; set; }
        public string PartyIdentifier { get; set; }
        public PartyIdTypes PartyIdentifierType { get; set; }
    }
}
