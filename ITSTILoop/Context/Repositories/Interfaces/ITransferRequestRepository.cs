using ITSTILoop.Model;
using ITSTILoop.Model.Interfaces;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Context.Repositories
{
    public interface ITransferRequestRepository : IGenericRepository<TransferRequest>
    {        
        TransferRequestResponseDTO CreateTransferRequest(TransferRequestDTO transferRequestDTO, PartyDTO partyDTO, int fromParticipantId);
        TransferRequestResponseDTO? RetrieveTransferRequest(Guid transferId);
    }
}