using ITSTILoop.Model;
using ITSTILoopLibrary.DTO;

namespace ITSTILoop.Context.Repositories.Interfaces
{
    public interface ITransferRequestRepository : IGenericRepository<TransferRequest>
    {        
        TransferRequestResponseDTO CreateTransferRequest(TransferRequestDTO transferRequestDTO, PartyDTO partyDTO, int fromParticipantId);
        TransferRequestResponseDTO? RetrieveTransferRequest(Guid transferId);
    }
}