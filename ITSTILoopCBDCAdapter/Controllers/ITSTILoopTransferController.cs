using ITSTILoopLibrary.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITSTILoopCBDCAdapter.Controllers
{

    [Route("itstiloop/[controller]")]
    [ApiController]
    public class ITSTILoopTransferController : ControllerBase
    {       
        private readonly ILogger<ITSTILoopTransferController> _logger;        

        public ITSTILoopTransferController(ILogger<ITSTILoopTransferController> logger)
        {
            _logger = logger;            
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransferRequestCompleteDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost(Name = "confirmTransfer")]
        public ActionResult<TransferRequestCompleteDTO> Post(TransferRequestResponseDTO transferRequestResponseDTO)
        {
            _logger.LogInformation($"ITSTILoopTransferController-Post-{transferRequestResponseDTO.To.PartyIdentifier.Identifier}");
            var partyIdentifierDTO = transferRequestResponseDTO.To.PartyIdentifier;
            //var account = _accountService.Accounts.FirstOrDefault(k => k.PartyDefinition.PartyIdentifier.Identifier == partyIdentifierDTO.Identifier && k.PartyDefinition.PartyIdentifier.PartyIdentifierType == partyIdentifierDTO.PartyIdentifierType);
            //if (account != null)
            //{
            //    account.TransferIn(transferRequestResponseDTO.Amount); 
            //    return new TransferRequestCompleteDTO() { TransferId = transferRequestResponseDTO.TransferId, Fullfilment = "Fullfilled" };
            //}
            return NotFound();
        }
    }
}
