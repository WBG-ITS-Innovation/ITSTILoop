using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Services.Interfaces;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Services
{
    public enum ParticipantConfirmTransferServiceResults { Success, UriMalformed, EndpointError, ParticipantNotRegistered, PartyNotFound };
    public class ParticipantConfirmTransferServiceResult
    {
        public ParticipantConfirmTransferServiceResults Result { get; set; }
        public TransferRequestCompleteDTO TransferRequestComplete { get; set; }
    }

    public class ConfirmTransferService : IConfirmTransferService
    {
        private readonly ILogger<ConfirmTransferService> _logger;
        private readonly IParticipantRepository _participantRepository;
        private readonly IHttpPostClient _httpPostClient;

        public ConfirmTransferService(ILogger<ConfirmTransferService> logger, IParticipantRepository participantRepository, IHttpPostClient httpPostClient)
        {
            _logger = logger;
            _participantRepository = participantRepository;
            _httpPostClient = httpPostClient;
        }
        public async Task<ParticipantConfirmTransferServiceResult> ConfirmTransferAsync(TransferRequestResponseDTO transferRequestResponseDTO)
        {
            ParticipantConfirmTransferServiceResult result = new ParticipantConfirmTransferServiceResult() { Result = ParticipantConfirmTransferServiceResults.Success };
            var participant = _participantRepository.GetById(transferRequestResponseDTO.To.ParticipantId);
            if (participant != null)
            {
                var postResponse = await _httpPostClient.PostAsync<TransferRequestResponseDTO, TransferRequestCompleteDTO>(transferRequestResponseDTO, participant.ConfirmTransferEndpoint);
                if (postResponse.Result == HttpPostClientResults.Success)
                {
                    result.TransferRequestComplete = postResponse.ResponseContent;
                }
                else
                {
                    result.Result = ParticipantConfirmTransferServiceResults.EndpointError;
                }
            }
            else
            {
                result.Result = ParticipantConfirmTransferServiceResults.ParticipantNotRegistered;
            }

            return result;
        }
    }
}

