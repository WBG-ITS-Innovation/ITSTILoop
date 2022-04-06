namespace ITSTILoop.Model
{
    public class SettlementAccount
    {
        public int SettlementAccountId { get; set; }
        public int AccountId { get; set; }
        public string ParticipantName { get; set; } = String.Empty;
        public decimal NetSettlementAmount { get; set; }
    }
}
