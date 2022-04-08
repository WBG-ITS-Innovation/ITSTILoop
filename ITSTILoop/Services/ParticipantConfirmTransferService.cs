using ITSTILoop.Services.Interfaces;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Services
{
    public enum ParticipantConfirmTransferServiceResults { Success, UriMalformed, EndpointError, ParticipantNotRegistered, PartyNotFound};
    public class ParticipantConfirmTransferServiceResult
    {
        public ParticipantConfirmTransferServiceResults Result { get; set; }
        public TransferRequestCompleteDTO TransferRequestComplete { get; set; }
    }

    public class ParticipantConfirmTransferService : IParticipantConfirmTransferService
    {
        private readonly ILogger<ParticipantConfirmTransferService> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public ParticipantConfirmTransferService(ILogger<ParticipantConfirmTransferService> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }


        public async Task<ParticipantConfirmTransferServiceResult> ConfirmTransferAsync(TransferRequestResponseDTO transferDTO, Uri endpoint)
        {
            ParticipantConfirmTransferServiceResult result = new ParticipantConfirmTransferServiceResult();
            var client = _clientFactory.CreateClient();
            if (client != null)
            {
                var httpResult = await client.PostAsJsonAsync<TransferRequestResponseDTO>(endpoint, transferDTO);
                if (httpResult.StatusCode == System.Net.HttpStatusCode.Accepted || httpResult.IsSuccessStatusCode)
                {
                    var contentResult = await httpResult.Content.ReadFromJsonAsync<TransferRequestCompleteDTO>();
                    result.TransferRequestComplete = contentResult;
                    result.Result = ParticipantConfirmTransferServiceResults.Success;
                }
                else
                {
                    var contentResult = await httpResult.Content.ReadAsStringAsync();
                    _logger.LogError("ConfirmTransferAsync-" + httpResult.StatusCode + "-" + httpResult.ReasonPhrase + "-" + contentResult);
                    result.Result = ParticipantConfirmTransferServiceResults.EndpointError;
                }
            }
            else
            {
                result.Result = ParticipantConfirmTransferServiceResults.UriMalformed;
            }
            return result;
        }
    }
}
