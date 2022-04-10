using ITSTILoopDTOLibrary;
using ITSTILoopSampleFSP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITSTILoopSampleFSP.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ExternalTransfersController : ControllerBase
    {
      

        private readonly ILogger<ExternalTransfersController> _logger;
        private readonly IHttpPostClient _httpPostClient;
        private readonly AccountService _accountService;

        public ExternalTransfersController(ILogger<ExternalTransfersController> logger, IHttpPostClient httpPostClient, AccountService accountService)
        {
            _logger = logger;
            _httpPostClient = httpPostClient;
            _accountService = accountService;
        }

        [HttpGet]
        public IEnumerable<TransferRequestResponseDTO> Get()
        {
            return _accountService.TransferRequests.Values.ToList();
        }

        [HttpPost(Name = "InitiateTransfer")]
        public async Task<ActionResult<TransferRequestResponseDTO>> PostAsync(TransferRequestDTO transferRequestDTO)
        {
            
            var response = await _httpPostClient.PostAsync<TransferRequestDTO, TransferRequestResponseDTO>(transferRequestDTO, "/Transfer", "itstiloop");
            if (response.Result == HttpPostClientResults.Success)
            {
                _accountService.TransferRequests.Add(response.ResponseContent.TransferId, response.ResponseContent);
                
                return response.ResponseContent;
            }
            return Problem(response.Result.ToString());
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
