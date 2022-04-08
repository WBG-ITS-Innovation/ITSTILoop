using ITSTILoopDTOLibrary;

namespace ITSTILoop.Services.Interfaces
{
    public interface IConfirmTransferService
    {
        Task<ParticipantConfirmTransferServiceResult> ConfirmTransferAsync(TransferRequestResponseDTO transferRequestResponseDTO);
    }
}