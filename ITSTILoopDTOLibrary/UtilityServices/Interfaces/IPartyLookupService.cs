using ITSTILoopLibrary.DTO;


namespace ITSTILoopLibrary.UtilityServices.Interfaces
{
    public class PartyLookupServiceResult
    {
        public PartyLookupServiceResults Result { get; set; }
        public PartyDTO FoundParty { get; set; }
    }

    public interface IPartyLookupService
    {
        Task<PartyLookupServiceResult> FindPartyAsync(PartyIdTypes partyIdType, string partyIdentifier);
    }
}