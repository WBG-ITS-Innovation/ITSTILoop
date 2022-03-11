namespace ITSTILoop.Model
{
    public class Participant
    {
        public int ParticipantId { get; set; }
        public string Name { get; set; }
        public Uri PartyLookupEndpoint { get; set; }
        public List<Account> Accounts { get; set; }
        public List<Party> Parties { get; set; }
        public string ApiKey { get; set; }
    }
}
