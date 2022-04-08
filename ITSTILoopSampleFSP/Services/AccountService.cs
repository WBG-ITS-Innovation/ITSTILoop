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
            //let's seed some accounts
            Accounts.Add(new FspAccount() { Balance = 10000, PartyDefinition = new PartyDTO() { FirstName = "Mert", LastName = "Ozdag", PartyIdentifier = new PartyIdentifierDTO() { Identifier = "5551234567", PartyIdentifierType = PartyIdTypes.MSISDN }, ParticipantId = participantId } });
            Accounts.Add(new FspAccount() { Balance = 20000, PartyDefinition = new PartyDTO() { FirstName = "Ava", LastName = "Jeay", PartyIdentifier = new PartyIdentifierDTO() { Identifier = "5551234568", PartyIdentifierType = PartyIdTypes.MSISDN }, ParticipantId = participantId } });
        }
    }
}
