using ITSTILoopDTOLibrary;

namespace ITSTILoop.Services.Interfaces
{
    public interface IParticipantConfirmTransferService
    {
        Task<ParticipantConfirmTransferServiceResult> ConfirmTransferAsync(TransferRequestResponseDTO transferDTO, Uri endpoint);
    }
}