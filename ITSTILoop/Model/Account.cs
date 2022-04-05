using ITSTILoopDTOLibrary;

namespace ITSTILoop.Model
{

    public class Account
    {
        public int AccountId { get; set; }
        public CurrencyCodes Currency { get; set; }
        public Decimal Position { get; set; }
        public Decimal Settlement { get; set; }
        public Decimal AvailableFunds
        {
            get
            {
                return Settlement + Position;
            }
        }
        public void FundsIn(decimal fund)
        {
            Settlement = Settlement + fund;
        }

        public void FundsOut(decimal fund)
        {
            Settlement = Settlement - fund;
        }

        public void TransferOut(decimal transferAmount)
        {
            Position = Position - transferAmount;
        }

        public void TransferIn(decimal transferAmount)
        {
            Position = Position + transferAmount;
        }

        public void SettleIn(decimal netSettlement)
        {
            Position = Position - netSettlement;
            Settlement = Settlement + netSettlement;
        }
    }
}
