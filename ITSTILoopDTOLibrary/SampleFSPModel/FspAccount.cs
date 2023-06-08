using ITSTILoopLibrary.DTO;

namespace ITSTILoopSampleFSP.Models
{
    public class FspAccount
    {
        public List<FspTransaction> Transactions { get; set; } = new List<FspTransaction>();
        public Decimal Balance { get; set; }
        public PartyDTO PartyDefinition { get; set; } = new PartyDTO();
        public void Deposit(decimal amount)
        {
            Balance += amount;
            Transactions.Add(new FspTransaction() { Amount = amount, Timestamp = DateTime.Now, TransactionType = TransactionTypes.Deposit });
        }
        public void TransferOut(decimal amount)
        {
            Balance -= amount;
            Transactions.Add(new FspTransaction() { Amount = amount, Timestamp = DateTime.Now, TransactionType = TransactionTypes.MoneySent });
        }

        public void TransferIn(decimal amount)
        {
            Balance += amount;
            Transactions.Add(new FspTransaction() { Amount = amount, Timestamp = DateTime.Now, TransactionType = TransactionTypes.MoneyRecieved });
        }
    }
}
