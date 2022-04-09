using ITSTILoop.DTO;
using ITSTILoop.Model;
using ITSTILoop.Services.Interfaces;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Services
{
    public enum PartyLookupServiceResults { Success, UriMalformed, EndpointError, ParticipantNotRegistered, PartyNotFound};
    public class PartyLookupServiceResult
    {
        public PartyLookupServiceResults Result { get; set; }
        public PartyDTO FoundParty { get; set; }
    }

    public class ParticipantPartyQueryService : IParticipantPartyQueryService
    {
        private readonly ILogger<ParticipantPartyQueryService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpPostClient _httpPostClient;

        public ParticipantPartyQueryService(ILogger<ParticipantPartyQueryService> logger, IHttpClientFactory clientFactory, IHttpPostClient httpPostClient)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _httpPostClient = httpPostClient;
        }


        public async Task<PartyLookupServiceResult> LookupPartyAsync(PartyIdentifierDTO partyQuery, Uri endpoint)
        {
            PartyLookupServiceResult result = new PartyLookupServiceResult();
            var postResult = await _httpPostClient.PostAsync<PartyIdentifierDTO, PartyDTO>(partyQuery, endpoint);
            result.Result = (PartyLookupServiceResults)postResult.Result;
            if (postResult.Result == HttpPostClientResults.Success)
            {
                result.FoundParty = postResult.ResponseContent;   
            }
            else
            {
                if (postResult.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    result.Result = PartyLookupServiceResults.PartyNotFound;
                }
            }
            return result;
            
        }


    }
}
