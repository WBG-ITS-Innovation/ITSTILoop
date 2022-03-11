using System;

namespace ITSTILoop.Model
{
    public enum TransactionTypes { Deposit, Withdrawal, Transfer, Settlement}
    public class Transaction
    {
        public int TransactionId { get; set; }
        public TransactionTypes TransactionType { get; set; }
        public Decimal Amount { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid TraceId { get; set; }
    }
}
