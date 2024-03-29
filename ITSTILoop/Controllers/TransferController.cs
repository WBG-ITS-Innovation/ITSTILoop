using AutoMapper;
using ITSTILoop.Attributes;
using ITSTILoop.Context;
using ITSTILoop.DTO;
using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Model;
using Microsoft.AspNetCore.Mvc;
using ITSTILoop.Services;
using ITSTILoop.Services.Interfaces;
using ITSTILoop.Context.Repositories;
using ITSTILoopLibrary.DTO;
using ITSTILoopLibrary.UtilityServices;
using ITSTILoopLibrary.UtilityServices.Interfaces;

namespace ITSTILoop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiKey]
    public class TransferController : ControllerBase
    {
        private readonly ILogger<TransferController> _logger;
        private readonly IPartyLookupService _partyLookupService;
        private readonly ITransferRequestRepository _transferRequestRepository;
        private readonly IConfirmTransferService _confirmTransferService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly ISettlementWindowRepository _settlementWindowRepository;

        public TransferController(ILogger<TransferController> logger, IPartyLookupService partyLookupService, ITransferRequestRepository transferRequestRepository, IConfirmTransferService confirmTransferService, ITransactionRepository transactionRepository, IParticipantRepository participantRepository, ISettlementWindowRepository settlementWindowRepository)
        {
            _logger = logger;
            _partyLookupService = partyLookupService;
            _transferRequestRepository = transferRequestRepository;
            _confirmTransferService = confirmTransferService;
            _transactionRepository = transactionRepository;
            _participantRepository = participantRepository;
            _settlementWindowRepository = settlementWindowRepository;
        }

        [HttpGet]
        public IEnumerable<TransferRequest> GetTransfers()
        {
            return _transferRequestRepository.GetAll();
        }

        [HttpPost]
        public async Task<ActionResult<TransferRequestResponseDTO>> PostAsync(TransferRequestDTO transferRequestDTO)
        {
            Participant? participant = _participantRepository.GetParticipantFromApiKeyId(Request.Headers);
            if (participant != null)
            {
                var partyLookupResult = await _partyLookupService.FindPartyAsync(transferRequestDTO.To.PartyIdentifierType, transferRequestDTO.To.Identifier);
                if (partyLookupResult.Result == PartyLookupServiceResults.Success)
                {
                    var toParticipant = _participantRepository.GetParticipantByName(partyLookupResult.FoundParty.PSPName);
                    if (toParticipant != null)
                    {
                        partyLookupResult.FoundParty.ParticipantId = toParticipant.ParticipantId;
                        var transferRequestResponse = _transferRequestRepository.CreateTransferRequest(transferRequestDTO, partyLookupResult.FoundParty, participant.ParticipantId);
                        return transferRequestResponse;
                    }
                    else
                    {
                        return Problem("Couldn't find participant from name");
                    }
                }
                return Problem(partyLookupResult.Result.ToString());
            }
            return Problem("Either Admin or Participant Not Found");
            
        }

        [HttpPost("{transferId}")]
        public async Task<ActionResult<TransferRequestCompleteDTO>> Confirm(Guid transferId, [FromBody] TransferAcceptRejectDTO transferAcceptRejectDTO)
        {
            _logger.LogInformation($"Confirm-ENTRY-{transferId}-{transferAcceptRejectDTO.AcceptTransfer}");
            Participant? participant = _participantRepository.GetParticipantFromApiKeyId(Request.Headers);
            if (participant != null)
            {
                TransferRequestResponseDTO? transferRequestResponseDTO = _transferRequestRepository.RetrieveTransferRequest(transferId);
                if (transferRequestResponseDTO != null)
                {
                    _logger.LogInformation($"Confirm-{transferRequestResponseDTO.FromParticipantId}-{transferRequestResponseDTO.To.ParticipantId}-{transferRequestResponseDTO.To.CbdcAddress}-{transferRequestResponseDTO.To.PSPName}");
                    if (participant.ParticipantId == transferRequestResponseDTO.FromParticipantId)
                    {
                        //TODO:Let's check balances
                        var result = await _confirmTransferService.ConfirmTransferAsync(transferRequestResponseDTO);
                        if (result.Result == ParticipantConfirmTransferServiceResults.Success)
                        {
                            _transactionRepository.MakeTransfer(transferRequestResponseDTO.FromParticipantId, transferRequestResponseDTO.To.ParticipantId, transferRequestResponseDTO.Amount, transferRequestResponseDTO.Currency, transferRequestResponseDTO.TransferId);
                            _settlementWindowRepository.UpdateSettlementWindow();
                            return result.TransferRequestComplete;
                        }
                        else
                        {
                            return Problem(result.Result.ToString());
                        }
                    }
                    return Problem("Not your transfer");
                }
                return Problem("TransferNotFound");
            }
            return Problem("Either Admin or Participant Not Found");
        }
    }
}