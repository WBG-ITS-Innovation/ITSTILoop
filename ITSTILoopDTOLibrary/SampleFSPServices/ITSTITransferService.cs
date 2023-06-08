using ITSTILoopLibrary.DTO;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ITSTILoopLibrary.SampleFSPServices
{
    public enum ITSTITransferServiceResults { Success, UriMalformed, EndpointError, ParticipantNotRegistered, PartyNotFound };
    public class StartTransferResult
    {
        public ITSTITransferServiceResults Result { get; set; }
        public TransferRequestResponseDTO RequestResponse { get; set; }
    }

    public class ConfirmTransferResult
    {
        public ITSTITransferServiceResults Result { get; set; }
        public TransferRequestCompleteDTO CompleteDTO { get; set; }
    }
    public class ITSTITransferService
    {
        private readonly ILogger<ITSTITransferService> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public ITSTITransferService(ILogger<ITSTITransferService> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }


        public async Task<StartTransferResult> StartTransfer(TransferRequestDTO transferRequestDTO, Uri endpoint)
        {
            StartTransferResult result = new StartTransferResult();
            var client = _clientFactory.CreateClient();
            if (client != null)
            {
                var httpResult = await client.PostAsJsonAsync<TransferRequestDTO>(endpoint, transferRequestDTO);
                if (httpResult.StatusCode == System.Net.HttpStatusCode.Accepted || httpResult.IsSuccessStatusCode)
                {
                    var contentResult = await httpResult.Content.ReadFromJsonAsync<TransferRequestResponseDTO>();
                    result.RequestResponse = contentResult;
                    result.Result = ITSTITransferServiceResults.Success;
                }
                else
                {
                    var contentResult = await httpResult.Content.ReadAsStringAsync();
                    _logger.LogError("StartTransfer-" + httpResult.StatusCode + "-" + httpResult.ReasonPhrase + "-" + contentResult);
                    result.Result = ITSTITransferServiceResults.EndpointError;
                }
            }
            else
            {
                result.Result = ITSTITransferServiceResults.UriMalformed;
            }
            return result;
        }

        public async Task<ConfirmTransferResult> AcceptTransfer(Guid transferId, TransferAcceptRejectDTO transferAcceptRejectDTO, Uri endpoint)
        {
            ConfirmTransferResult result = new ConfirmTransferResult();
            var client = _clientFactory.CreateClient();
            if (client != null)
            {
                var httpResult = await client.PutAsJsonAsync<TransferAcceptRejectDTO>(endpoint, transferAcceptRejectDTO);
                if (httpResult.StatusCode == System.Net.HttpStatusCode.Accepted || httpResult.IsSuccessStatusCode)
                {
                    var contentResult = await httpResult.Content.ReadFromJsonAsync<TransferRequestCompleteDTO>();
                    result.CompleteDTO = contentResult;
                    result.Result = ITSTITransferServiceResults.Success;
                }
                else
                {
                    var contentResult = await httpResult.Content.ReadAsStringAsync();
                    _logger.LogError("AcceptTransfer-" + httpResult.StatusCode + "-" + httpResult.ReasonPhrase + "-" + contentResult);
                    result.Result = ITSTITransferServiceResults.EndpointError;
                }
            }
            else
            {
                result.Result = ITSTITransferServiceResults.UriMalformed;
            }
            return result;
        }
    }
}
