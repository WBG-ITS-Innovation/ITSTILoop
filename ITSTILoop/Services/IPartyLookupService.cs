using ITSTILoop.DTO;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Services
{
    public interface IPartyLookupService
    {
        Task<PartyLookupServiceResult> LookupPartyAsync(QueryPartyDTO partyQuery, Uri endpoint);
    }
}