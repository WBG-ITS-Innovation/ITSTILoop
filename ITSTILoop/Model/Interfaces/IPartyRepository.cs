using ITSTILoop.Model;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Model.Interfaces
{
    public interface IPartyRepository : IGenericRepository<Party>
    {
        Party? GetPartyFromTypeAndId(PartyIdTypes partyIdentifierType, string partyIdentifier);
    }
}
