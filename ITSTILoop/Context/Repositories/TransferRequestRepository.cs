using AutoMapper;
using ITSTILoop.Model;
using ITSTILoop.Model.Interfaces;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Context.Repositories
{
    public class TransferRequestRepository : GenericRepository<TransferRequestRepository>, ITransferRequestRepository
    {
        private readonly IMapper _mapper;

        public TransferRequestRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public TransferRequestResponseDTO CreateTransferRequest(TransferRequestDTO transferRequestDTO, PartyDTO partyDTO)
        {
            TransferRequest transferRequest = _mapper.Map<TransferRequest>(transferRequestDTO);
            transferRequest.FromPartyIdentifier = transferRequestDTO.From.PartyIdentifier;
            transferRequest.FromPartyIdentifierType = transferRequestDTO.From.PartyIdentifierType;
            transferRequest.ToPartyIdentifier = transferRequestDTO.To.PartyIdentifier;
            transferRequest.ToPartyIdentifierType = transferRequestDTO.To.PartyIdentifierType;
            transferRequest.FirstName = partyDTO.FirstName;
            transferRequest.LastName = partyDTO.LastName;
            transferRequest.InitiatedTimestamp = DateTime.Now;
            transferRequest.ToRegisteredParticipantName = partyDTO.RegisteredParticipantName;
            transferRequest.TransferId = Guid.NewGuid();
            _context.TransferRequests.Add(transferRequest);
            Save();
            var response = _mapper.Map<TransferRequestResponseDTO>(transferRequestDTO);
            response.To = partyDTO;
            response.TransferId = transferRequest.TransferId;
            response.InitiatedTimestamp = transferRequest.InitiatedTimestamp;
            return response;
        }
    }
}
