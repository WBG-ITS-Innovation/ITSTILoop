using ITSTILoopDTOLibrary;

namespace ITSTILoop.Services.Interfaces
{
    public interface IPartyLookupService
    {
        Task<PartyLookupServiceResult> FindPartyAsync(PartyIdTypes partyIdType, string partyIdentifier);
    }
}