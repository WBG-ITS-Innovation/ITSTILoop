using ITSTILoop.DTO;
using ITSTILoop.Model;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Services
{
    public enum PartyLookupServiceResults { Success, UriMalformed, EndpointError};
    public class PartyLookupServiceResult
    {
        public PartyLookupServiceResults Result { get; set; }
        public PartyDTO FoundParty { get; set; }
    }

    public class PartyLookupService : IPartyLookupService
    {
        private readonly ILogger<PartyLookupService> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public PartyLookupService(ILogger<PartyLookupService> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }


        public async Task<PartyLookupServiceResult> LookupPartyAsync(QueryPartyDTO partyQuery, Uri endpoint)
        {
            PartyLookupServiceResult result = new PartyLookupServiceResult();
            var client = _clientFactory.CreateClient();
            if (client != null)
            {
                var httpResult = await client.PostAsJsonAsync<QueryPartyDTO>(endpoint, partyQuery);
                if (httpResult.StatusCode == System.Net.HttpStatusCode.Accepted || httpResult.IsSuccessStatusCode)
                {
                    var contentResult = await httpResult.Content.ReadFromJsonAsync<PartyDTO>();
                    result.FoundParty = contentResult;
                    result.Result = PartyLookupServiceResults.Success;
                }
                else
                {
                    var contentResult = await httpResult.Content.ReadAsStringAsync();
                    _logger.LogError("LookupPartyAsync-" + httpResult.StatusCode + "-" + httpResult.ReasonPhrase + "-" + contentResult);
                    result.Result = PartyLookupServiceResults.EndpointError;
                }
            }
            else
            {
                result.Result = PartyLookupServiceResults.UriMalformed;
            }
            return result;
        }
    }
}
