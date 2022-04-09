using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Services.Interfaces;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Services
{
    public class ConfirmTransferService : IConfirmTransferService
    {
        private readonly ILogger<ConfirmTransferService> _logger;
        private readonly IParticipantRepository _participantRepository;
        private readonly IParticipantConfirmTransferService _participantConfirmTransferService;

        public ConfirmTransferService(ILogger<ConfirmTransferService> logger, IParticipantRepository participantRepository, IParticipantConfirmTransferService participantConfirmTransferService)
        {
            _logger = logger;
            _participantRepository = participantRepository;
            _participantConfirmTransferService = participantConfirmTransferService;
        }
        public async Task<ParticipantConfirmTransferServiceResult> ConfirmTransferAsync(TransferRequestResponseDTO transferRequestResponseDTO)
        {
            ParticipantConfirmTransferServiceResult result = new ParticipantConfirmTransferServiceResult() { Result = ParticipantConfirmTransferServiceResults.Success };
            var participant = _participantRepository.GetById(transferRequestResponseDTO.To.ParticipantId);
            if (participant != null)
            {
                result = await _participantConfirmTransferService.ConfirmTransferAsync(transferRequestResponseDTO, participant.ConfirmTransferEndpoint);
            }
            else
            {
                result.Result = ParticipantConfirmTransferServiceResults.ParticipantNotRegistered;
            }

            return result;
        }
    }
}

