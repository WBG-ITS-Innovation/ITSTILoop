namespace ITSTILoopSampleFSP.Models
{
    public enum TransactionTypes { Deposit, MoneySent, MoneyRecieved}
    public class FspTransaction
    {
        public DateTime Timestamp { get; set; }
        public TransactionTypes TransactionType { get; set; }
        public Decimal Amount { get; set; }

    }
}
