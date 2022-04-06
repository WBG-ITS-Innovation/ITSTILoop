using ITSTILoop.Model;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Context.Repositories
{
    public interface ITransferRequestRepository
    {
        TransferRequestResponseDTO CreateTransferRequest(TransferRequestDTO transferRequestDTO, PartyDTO partyDTO);
    }
}