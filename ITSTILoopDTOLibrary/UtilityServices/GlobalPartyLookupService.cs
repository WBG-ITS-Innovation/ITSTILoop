using ITSTILoopLibrary.DTO;
using ITSTILoopLibrary.Utility;
using ITSTILoopLibrary.UtilityServices.Interfaces;
using Microsoft.Extensions.Logging;

namespace ITSTILoopLibrary.UtilityServices
{
    public enum PartyLookupServiceResults { Success, UriMalformed, EndpointError, ParticipantNotRegistered, PartyNotFound };


    public class GlobalPartyLookupService : IPartyLookupService
    {
        private readonly ILogger<GlobalPartyLookupService> _logger;
        private readonly IHttpPostClient _httpPostClient;

        public GlobalPartyLookupService(ILogger<GlobalPartyLookupService> logger, IHttpPostClient httpPostClient)
        {
            _logger = logger;
            _httpPostClient = httpPostClient;

        }
        public async Task<PartyLookupServiceResult> FindPartyAsync(PartyIdTypes partyIdType, string partyIdentifier)
        {
            PartyLookupServiceResult result = new PartyLookupServiceResult();

            PartyIdentifierDTO queryPartyDTO = new PartyIdentifierDTO() { Identifier = partyIdentifier, PartyIdentifierType = partyIdType };
            //_httpPostClient.
            var postResponse = await _httpPostClient.PostAsync<PartyIdentifierDTO, PartyDTO>(queryPartyDTO, EnvironmentVariables.GetEnvironmentVariable(EnvironmentVariableNames.GLOBAL_ADDRESS_LOOKUP_URL, EnvironmentVariableDefaultValues.GLOBAL_ADDRESS_LOOKUP_URL_DEFAULT_VALUE));
            if (postResponse.Result == HttpPostClientResults.Success)
            {
                result.FoundParty = postResponse.ResponseContent;
            }
            else
            {
                result.Result = PartyLookupServiceResults.EndpointError;
            }
            return result;
        }
    }
}

