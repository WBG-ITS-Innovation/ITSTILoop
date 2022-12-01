using ITSTILoopLibrary.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CBDCTransferContract;

namespace ITSTILoopCBDCAdapter.Controllers
{

    [Route("itstiloop/[controller]")]
    [ApiController]
    public class ITSTILoopTransferController : ControllerBase
    {       
        private readonly ILogger<ITSTILoopTransferController> _logger;
        private readonly CBDCTransferService _cBDCBridgeService;

        public ITSTILoopTransferController(ILogger<ITSTILoopTransferController> logger, CBDCTransferService cBDCBridgeService)
        {
            _logger = logger;
            _cBDCBridgeService = cBDCBridgeService;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransferRequestCompleteDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost(Name = "confirmTransfer")]
        public async Task<ActionResult<TransferRequestCompleteDTO>> PostAsync(TransferRequestResponseDTO transferRequestResponseDTO)
        {
            _logger.LogInformation($"PostAsync-ENTRY-{transferRequestResponseDTO.To.PartyIdentifier.Identifier}-{transferRequestResponseDTO.To.CbdcAddress}-{transferRequestResponseDTO.To.PSPName}");
            PartyDTO partyDTO = transferRequestResponseDTO.To;
            var receipt = await _cBDCBridgeService.MakeCBDCTransfer(transferRequestResponseDTO.From.Identifier, partyDTO.CbdcAddress, (int)transferRequestResponseDTO.Amount, "itstiloop");
            if (!String.IsNullOrEmpty(receipt))
            {
                return new TransferRequestCompleteDTO() { TransferId = transferRequestResponseDTO.TransferId, Fullfilment = "Fullfilled" };
            }
            else
            {
                return Problem();
            }
        }
    }
}
