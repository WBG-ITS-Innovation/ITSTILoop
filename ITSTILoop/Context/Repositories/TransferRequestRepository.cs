using AutoMapper;
using ITSTILoop.Model;
using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Context.Repositories
{
    public class TransferRequestRepository : GenericRepository<TransferRequest>, ITransferRequestRepository
    {
        private readonly IMapper _mapper;

        public TransferRequestRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public TransferRequestResponseDTO CreateTransferRequest(TransferRequestDTO transferRequestDTO, PartyDTO partyDTO, int fromParticipantId)
        {
            TransferRequest transferRequest = _mapper.Map<TransferRequest>(transferRequestDTO);
            transferRequest.FromPartyIdentifier = transferRequestDTO.From.Identifier;
            transferRequest.FromPartyIdentifierType = transferRequestDTO.From.PartyIdentifierType;
            transferRequest.ToPartyIdentifier = transferRequestDTO.To.Identifier;
            transferRequest.ToPartyIdentifierType = transferRequestDTO.To.PartyIdentifierType;
            transferRequest.FirstName = partyDTO.FirstName;
            transferRequest.LastName = partyDTO.LastName;
            transferRequest.InitiatedTimestamp = DateTime.Now;
            transferRequest.ToParticipantId = partyDTO.ParticipantId;
            transferRequest.FromParticipantId = fromParticipantId;
            transferRequest.TransferId = Guid.NewGuid();
            _context.TransferRequests.Add(transferRequest);
            Save();
            var response = _mapper.Map<TransferRequestResponseDTO>(transferRequestDTO);
            response.To = partyDTO;
            response.TransferId = transferRequest.TransferId;
            response.InitiatedTimestamp = transferRequest.InitiatedTimestamp;
            response.FromParticipantId = fromParticipantId;
            return response;
        }

        public TransferRequestResponseDTO? RetrieveTransferRequest(Guid transferId)
        {
            var transferRequest = _context.TransferRequests.FirstOrDefault(k => k.TransferId == transferId);
            if (transferRequest != null)
            {
                var transferRequestDTO = _mapper.Map<TransferRequestResponseDTO>(transferRequest);
                transferRequestDTO.From.Identifier = transferRequest.FromPartyIdentifier;
                transferRequestDTO.From.PartyIdentifierType = transferRequest.FromPartyIdentifierType;
                transferRequestDTO.To.PartyIdentifier.Identifier = transferRequest.ToPartyIdentifier;
                transferRequestDTO.To.PartyIdentifier.PartyIdentifierType = transferRequest.ToPartyIdentifierType;
                transferRequestDTO.To.ParticipantId = transferRequest.ToParticipantId;
                transferRequestDTO.To.FirstName = transferRequest.FirstName;
                transferRequestDTO.To.LastName = transferRequest.LastName;
                transferRequestDTO.FromParticipantId = transferRequest.FromParticipantId;
                return transferRequestDTO;
            }
            else
            {
                return null;
            }
        }
    }
}
