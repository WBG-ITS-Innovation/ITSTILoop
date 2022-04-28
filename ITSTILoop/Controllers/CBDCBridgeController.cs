using CBDCHubContract.Services;
using ITSTILoop.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace ITSTILoop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiKey]
    public class CBDCBridgeController : ControllerBase
    {
        private readonly CBDCBridgeService _bridgeService;
        private readonly ILogger<CBDCBridgeController> _logger;

        public CBDCBridgeController(ILogger<CBDCBridgeController> logger, CBDCBridgeService cBDCBridgeService)
        {
            _bridgeService = cBDCBridgeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<RegisteredFspDTO>> GetRegisteredFSPs()
        {
            return await _bridgeService.GetRegisteredFSPAsync();
        }

        [HttpPost("{participantId}")]
        public async Task<ActionResult> MapFSPs(int participantId, string cbdcAddress)
        {
            return Ok();
        }
    }
}
