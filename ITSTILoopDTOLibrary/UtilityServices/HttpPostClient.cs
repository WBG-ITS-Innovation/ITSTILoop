using ITSTILoopLibrary.UtilityServices.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace ITSTILoopLibrary.UtilityServices
{
    public enum HttpPostClientResults { Success, UriMalformed, EndpointError };
    public class HttpPostClientResponse<T>
    {
        public T ResponseContent { get; set; }
        public HttpPostClientResults Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class HttpPostClient : IHttpPostClient
    {
        private readonly ILogger<HttpPostClient> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private string _baseAddress;
        private HttpClient _client;

        public HttpPostClient(ILogger<HttpPostClient> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<HttpPostClientResponse<TResponseType>> PostAsync<TPostType, TResponseType>(TPostType postContent, Uri endpoint, string clientName = "")
        {
            return await PostAsync<TPostType, TResponseType>(postContent, endpoint.ToString(), clientName);
        }

        public async Task<HttpPostClientResponse<TResponseType>> PostAsync<TPostType, TResponseType>(TPostType postContent, string endpoint, string clientName = "")
        {
            HttpPostClientResponse<TResponseType> result = new HttpPostClientResponse<TResponseType>();
            HttpClient? client = null;
            if (String.IsNullOrEmpty(clientName))
            {
                client = _clientFactory.CreateClient();
            }
            else
            {
                client = _clientFactory.CreateClient(clientName);
            }

            if (client != null)
            {
                var httpResult = await client.PostAsJsonAsync<TPostType>(endpoint, postContent);
                result.StatusCode = httpResult.StatusCode;
                if (httpResult.StatusCode == System.Net.HttpStatusCode.Accepted || httpResult.IsSuccessStatusCode)
                {
                    var contentResult = await httpResult.Content.ReadFromJsonAsync<TResponseType>();
                    result.ResponseContent = contentResult;
                    result.Result = HttpPostClientResults.Success;
                }
                else
                {

                    var contentResult = await httpResult.Content.ReadAsStringAsync();
                    _logger.LogError("PostAsync-" + httpResult.StatusCode + "-" + httpResult.ReasonPhrase + "-" + contentResult);
                    result.Result = HttpPostClientResults.EndpointError;
                }
            }
            else
            {
                result.Result = HttpPostClientResults.UriMalformed;
            }
            return result;
        }
    }
}

