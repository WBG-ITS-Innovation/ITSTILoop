using ITSTILoopDTOLibrary;

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
        }
        public void TransferOut(decimal amount)
        {
            Balance -= amount;
        }

        public void TransferIn(decimal amount)
        {
            Balance += amount;
        }
    }
}
