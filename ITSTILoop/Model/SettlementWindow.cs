namespace ITSTILoop.Model
{
    public enum SettlementWindowStatuses { Open, Closed, Settled }
    public class SettlementWindow
    {
        public int SettlementWindowId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime ModifiedAt { get; set; } = DateTime.Now.ToUniversalTime();
        public SettlementWindowStatuses Status { get; set; } = SettlementWindowStatuses.Open;
        public List<SettlementAccount> SettlementAccounts { get; set; } = new List<SettlementAccount>();
    }
}
