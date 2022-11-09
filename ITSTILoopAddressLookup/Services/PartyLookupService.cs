using ITSTILoopDTOLibrary;

namespace ITSTILoopAddressLookup.Services
{
    public enum PartyLookupServiceResults { Success, UriMalformed, EndpointError, ParticipantNotRegistered, PartyNotFound };
    public class PartyLookupServiceResult
    {
        public PartyLookupServiceResults Result { get; set; }
        public GlobalPartyIdentifierDTO? FoundParty { get; set; }
    }

    public class PartyLookupService
    {
        private readonly ILogger<PartyLookupService> _logger;
        public List<GlobalPartyIdentifierDTO> Parties { get; set; } = new List<GlobalPartyIdentifierDTO>();

        public PartyLookupService(ILogger<PartyLookupService> logger)
        {
            _logger = logger;

        }
        public PartyLookupServiceResult FindParty(PartyIdTypes partyIdType, string partyIdentifier)
        {
            PartyLookupServiceResult result = new PartyLookupServiceResult() { Result = PartyLookupServiceResults.PartyNotFound };
            var p = Parties.FirstOrDefault(k => k.Identifier.PartyIdentifierType == partyIdType && k.Identifier.Identifier == partyIdentifier);
            if (p != null)
            {
                result.FoundParty = p;
                result.Result = PartyLookupServiceResults.Success;
            }
            return result;
        }
    }
}
