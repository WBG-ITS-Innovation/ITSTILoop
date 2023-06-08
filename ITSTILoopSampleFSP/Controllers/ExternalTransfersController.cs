using ITSTILoopLibrary.DTO;
using ITSTILoopLibrary.Utility;
using ITSTILoopLibrary.UtilityServices;
using ITSTILoopLibrary.UtilityServices.Interfaces;
using ITSTILoopSampleFSP.Services;
using Microsoft.AspNetCore.Mvc;
using CBDCTransferContract;

namespace ITSTILoopSampleFSP.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ExternalTransfersController : ControllerBase
    {
      

        private readonly ILogger<ExternalTransfersController> _logger;
        private readonly IHttpPostClient _httpPostClient;
        private readonly AccountService _accountService;
        private readonly CBDCTransferService _cBDCBridgeService;
        private readonly GlobalPartyLookupService _globalPartyLookupService;

        public ExternalTransfersController(ILogger<ExternalTransfersController> logger, IHttpPostClient httpPostClient, AccountService accountService, CBDCTransferService cBDCBridgeService, GlobalPartyLookupService globalPartyLookupService)
        {
            _logger = logger;
            _httpPostClient = httpPostClient;
            _accountService = accountService;
            _cBDCBridgeService = cBDCBridgeService;
            _globalPartyLookupService = globalPartyLookupService;
        }

        [HttpGet]
        public IEnumerable<TransferRequestResponseDTO> Get()
        {
            return _accountService.TransferRequests.Values.ToList();
        }

        [HttpPost(Name = "InitiateTransfer")]
        public async Task<ActionResult<TransferRequestResponseDTO>> PostAsync(TransferRequestDTO transferRequestDTO)
        {
            if (EnvironmentVariables.GetEnvironmentVariable(EnvironmentVariableNames.IS_LOOP_PARTICIPANT, EnvironmentVariableDefaultValues.IS_LOOP_PARTICIPANT_DEFAULT_VALUE).ToLower() == "true")
            {
                var response = await _httpPostClient.PostAsync<TransferRequestDTO, TransferRequestResponseDTO>(transferRequestDTO, "/Transfer", "itstiloop");
                if (response.Result == HttpPostClientResults.Success)
                {
                    _accountService.TransferRequests.Add(response.ResponseContent.TransferId, response.ResponseContent);

                    return Ok(response.ResponseContent);
                }
                return Problem(response.Result.ToString());
            }
            else
            {
                var lookupResult = await _globalPartyLookupService.FindPartyAsync(transferRequestDTO.To.PartyIdentifierType, transferRequestDTO.To.Identifier);
                if (lookupResult.Result == PartyLookupServiceResults.Success)
                {
                    var transferResult = await _cBDCBridgeService.MakeTransfer(transferRequestDTO.From.Identifier, transferRequestDTO.To.Identifier, (int)transferRequestDTO.Amount, lookupResult.FoundParty.HubName);
                    return Ok(new TransferRequestResponseDTO() { Amount = transferRequestDTO.Amount, CurrentState = TransferStates.WaitingForPartyAcceptance, From = transferRequestDTO.From, Note = transferResult });
                }
            }
            return Problem();
        }

        [HttpPost("{transferId}")]
        public async Task<ActionResult<TransferRequestCompleteDTO>> Confirm(Guid transferId)
        {
            if (_accountService.TransferRequests.ContainsKey(transferId))
            {
                TransferAcceptRejectDTO transferAcceptRejectDTO = new TransferAcceptRejectDTO() { AcceptTransfer = true };
                var response = await _httpPostClient.PostAsync<TransferAcceptRejectDTO, TransferRequestCompleteDTO>(transferAcceptRejectDTO, $"/Transfer/{transferId}", "itstiloop");
                if (response.Result == HttpPostClientResults.Success)
                {
                    var transferRequest = _accountService.TransferRequests[transferId];
                    transferRequest.CurrentState = TransferStates.Completed;
                    var account = _accountService.Accounts.FirstOrDefault(k => k.PartyDefinition.PartyIdentifier.Identifier == transferRequest.From.Identifier);
                    account.TransferOut(transferRequest.Amount);
                    return response.ResponseContent;
                }
                return Problem(response.Result.ToString());
            }
            return Problem("Transfer Doesn't exist");
        }


    }
}
