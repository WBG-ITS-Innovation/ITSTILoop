using ITSTILoop.Model;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Context.Repositories.Interfaces
{
    public interface IParticipantRepository : IGenericRepository<Participant>
    {
        Participant? GetParticipantFromApiKey(string apiId, string apiKey);
        void AddPartyToParticipant(int participantId, Party party);
        Participant? GetParticipantByName(string name);
        void FundParticipant(int participantId, CurrencyCodes currency, decimal amount);        
        Participant? GetParticipantFromApiKeyId(IHeaderDictionary requestHeaders);
        Participant CreateParticipant(string name, string apiId, string apiKey, Uri partyLookupEndpoint, Uri confirmTransferEndpoint, string cdbcAddress);
        Participant? GetByIdFull(int id);
        void FundParticipant(string cbdcAddress, decimal tokens);
        void ModifyParticipant(int participantId, CurrencyCodes currency, decimal position, decimal netSettlement);
    }
}
