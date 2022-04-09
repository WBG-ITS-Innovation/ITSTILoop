using ITSTILoop.Model;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Context.Repositories.Interfaces
{
    public interface IPartyRepository : IGenericRepository<Party>
    {
        Party? GetPartyFromTypeAndId(PartyIdTypes partyIdentifierType, string partyIdentifier);
    }
}
