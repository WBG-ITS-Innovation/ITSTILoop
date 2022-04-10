using ITSTILoopDTOLibrary;
using ITSTILoopSampleFSP.Models;

namespace ITSTILoopSampleFSP.Services
{
    public class AccountService
    {        
        public List<FspAccount> Accounts { get; set; } = new List<FspAccount>();
        public Dictionary<Guid, TransferRequestResponseDTO> TransferRequests = new Dictionary<Guid, TransferRequestResponseDTO>();

        public AccountService()
        {
            int participantId = Convert.ToInt32(Environment.GetEnvironmentVariable("PARTICIPANT_ID"));
            var accountsString = Environment.GetEnvironmentVariable("ACCOUNTS");
            var accountsStringSplit = accountsString.Split('|');
            foreach (var accountString in accountsStringSplit)
            {
                var accountStringSplit = accountString.Split(':');
                var initialBalance = Convert.ToDecimal(accountStringSplit[3]);
                var firstName = accountStringSplit[0];
                var lastName = accountStringSplit[1];
                var pi = accountStringSplit[2];
                Accounts.Add(new FspAccount() { Balance = initialBalance, PartyDefinition = new PartyDTO() { FirstName = firstName, LastName = lastName, PartyIdentifier = new PartyIdentifierDTO() { Identifier = pi, PartyIdentifierType = PartyIdTypes.MSISDN }, ParticipantId = participantId } });
            }
            //let's seed some accounts
            
            
        }
    }
}
