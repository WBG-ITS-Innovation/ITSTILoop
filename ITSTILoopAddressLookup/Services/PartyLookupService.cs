using ITSTILoopDTOLibrary;
using ITSTILoopLibrary.Utility;

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
            string partiesString = EnvironmentVariables.GetEnvironmentVariable(EnvironmentVariableNames.GLOBAL_ADDRESS_LOOKUP_PARTIES, String.Empty);
            if (!String.IsNullOrEmpty(partiesString))
            {
                string[] partiesStringSplit = partiesString.Split('|');
                foreach (var partyString in partiesStringSplit)
                {
                    string[] partyStringSplit = partyString.Split(':');
                    string partyIdentifier = partiesStringSplit[0];
                    string hubName = partiesStringSplit[1];
                    string fspName = partiesStringSplit[2];
                    GlobalPartyIdentifierDTO globalPartyIdentifierDTO = new GlobalPartyIdentifierDTO() { FSPName = fspName, HubName = hubName };
                    PartyIdentifierDTO partyIdentifierDto = new PartyIdentifierDTO() { Identifier = partyIdentifier, PartyIdentifierType = PartyIdTypes.MSISDN };
                    globalPartyIdentifierDTO.IdentifierDto = partyIdentifierDto;
                    Parties.Add(globalPartyIdentifierDTO);
                }
            }
        }
        public PartyLookupServiceResult FindParty(PartyIdTypes partyIdType, string partyIdentifier)
        {
            PartyLookupServiceResult result = new PartyLookupServiceResult() { Result = PartyLookupServiceResults.PartyNotFound };
            var p = Parties.FirstOrDefault(k => k.IdentifierDto.PartyIdentifierType == partyIdType && k.IdentifierDto.Identifier == partyIdentifier);
            if (p != null)
            {
                result.FoundParty = p;
                result.Result = PartyLookupServiceResults.Success;
            }
            return result;
        }
    }
}
