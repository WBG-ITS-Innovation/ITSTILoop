namespace ITSTILoop.Model
{
    public class SettlementAccount
    {
        private decimal _netSettlementAmount;
        public int SettlementAccountId { get; set; }
        public int AccountId { get; set; }
        public string ParticipantName { get; set; } = String.Empty;
        public decimal NetSettlementAmount {
            get 
            { 
                return _netSettlementAmount;
            }
            set 
            {
                _netSettlementAmount = value;
            }
        }
    }
}
