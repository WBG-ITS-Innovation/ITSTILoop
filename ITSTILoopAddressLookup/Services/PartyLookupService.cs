using ITSTILoopLibrary.DTO;
using ITSTILoopLibrary.Utility;

namespace ITSTILoopAddressLookup.Services
{
    public enum PartyLookupServiceResults { Success, UriMalformed, EndpointError, ParticipantNotRegistered, PartyNotFound };
    public class PartyLookupServiceResult
    {
        public PartyLookupServiceResults Result { get; set; }
        public PartyDTO? FoundParty { get; set; }
    }

    public class PartyLookupService
    {
        private readonly ILogger<PartyLookupService> _logger;
        public List<PartyDTO> Parties { get; set; } = new List<PartyDTO>();

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
                    string partyIdentifier = partyStringSplit[0];
                    string hubName = partyStringSplit[1];
                    string fspName = partyStringSplit[2];
                    PartyDTO globalPartyIdentifierDTO = new PartyDTO() { PSPName = fspName, HubName = hubName };
                    if (hubName.ToLower() == "cbdc")
                    {
                        globalPartyIdentifierDTO.CbdcAddress = fspName;
                        globalPartyIdentifierDTO.PSPName = "cbdc";
                    }
                    PartyIdentifierDTO partyIdentifierDto = new PartyIdentifierDTO() { Identifier = partyIdentifier, PartyIdentifierType = PartyIdTypes.MSISDN };
                    globalPartyIdentifierDTO.PartyIdentifier = partyIdentifierDto;
                    Parties.Add(globalPartyIdentifierDTO);
                }
            }
        }
        public PartyLookupServiceResult FindParty(PartyIdTypes partyIdType, string partyIdentifier)
        {
            _logger.LogInformation($"FindParty-ENTRY-{partyIdType}-{partyIdentifier}");
            PartyLookupServiceResult result = new PartyLookupServiceResult() { Result = PartyLookupServiceResults.PartyNotFound };
            var p = Parties.FirstOrDefault(k => k.PartyIdentifier.PartyIdentifierType == partyIdType && k.PartyIdentifier.Identifier == partyIdentifier);
            if (p != null)
            {
                result.FoundParty = p;
                result.Result = PartyLookupServiceResults.Success;
            }
            _logger.LogInformation($"FindParty-EXIT-{partyIdType}-{partyIdentifier}");
            return result;
        }
    }
}
