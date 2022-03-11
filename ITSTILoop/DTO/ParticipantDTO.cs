namespace ITSTILoop.DTO
{
    public class ParticipantDTO
    {
        public int ParticipantId { get; set; }
        public string Name { get; set; }        
        public Uri PartyLookupEndpoint { get; set; }
    }

    public class RegisterParticipantDTO
    {
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public Uri PartyLookupEndpoint { get; set; }
    }
}
