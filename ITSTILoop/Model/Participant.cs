using ITSTILoopDTOLibrary;

namespace ITSTILoop.Model
{
    public class Participant
    {
        public int ParticipantId { get; set; }
        public string Name { get; set; }
        public Uri PartyLookupEndpoint { get; set; }
        public Uri ConfirmTransferEndpoint { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<Party> Parties { get; set; } = new List<Party>();
        public string ApiKey { get; set; }
        
        public void FundAccount(CurrencyCodes currency, decimal amount)
        {
            Account? account = Accounts.FirstOrDefault(k => k.Currency == currency);
            if (account != null)
            {
                account.FundsIn(amount);
            }
            else
            {
                Account newAccount = new Account() { Currency = currency };
                newAccount.FundsIn(amount);
                Accounts.Add(newAccount);
            }
        }
    }
}
