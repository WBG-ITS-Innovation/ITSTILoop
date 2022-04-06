using AutoMapper;
using ITSTILoop.Attributes;
using ITSTILoop.Context;
using ITSTILoop.DTO;
using ITSTILoop.Model.Interfaces;
using ITSTILoop.Model;
using Microsoft.AspNetCore.Mvc;
using ITSTILoopDTOLibrary;
using ITSTILoop.Services;
using ITSTILoop.Services.Interfaces;
using ITSTILoop.Context.Repositories;

namespace ITSTILoop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiKey]
    public class TransferController : ControllerBase
    {
        private readonly ILogger<TransferController> _logger;
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;
        private readonly IPartyLookupService _partyLookupService;
        private readonly ITransferRequestRepository _transferRequestRepository;

        public TransferController(ILogger<TransferController> logger, IParticipantRepository participantRepository, IMapper mapper, IPartyLookupService partyLookupService, ITransferRequestRepository transferRequestRepository)
        {
            _logger = logger;
            _participantRepository = participantRepository;
            _mapper = mapper;
            _partyLookupService = partyLookupService;
            _transferRequestRepository = transferRequestRepository;
        }

        [HttpPost]
        public async Task<ActionResult<TransferRequestResponseDTO>> PostAsync(TransferRequestDTO transferRequestDTO)
        {
            var partyLookupResult = await _partyLookupService.FindPartyAsync(transferRequestDTO.To.PartyIdentifierType, transferRequestDTO.To.PartyIdentifier);
            if (partyLookupResult.Result == PartyLookupServiceResults.Success)
            {
                var transferRequestResponse = _transferRequestRepository.CreateTransferRequest(transferRequestDTO, partyLookupResult.FoundParty);
                return transferRequestResponse;
            }
            return Problem(partyLookupResult.Result.ToString());
            
        }

        [HttpPut("{transferId}")]
        public ActionResult<TransferRequestCompleteDTO> Put(Guid transferId, [FromBody] TransferAcceptRejectDTO transferAcceptRejectDTO)
        {
            return Ok();
        }
    }
}