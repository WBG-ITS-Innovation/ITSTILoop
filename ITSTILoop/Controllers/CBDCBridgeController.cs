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
        private readonly CBDCHubService _bridgeService;
        private readonly ILogger<CBDCBridgeController> _logger;

        public CBDCBridgeController(ILogger<CBDCBridgeController> logger, CBDCHubService cBDCBridgeService)
        {
            _bridgeService = cBDCBridgeService;
            _logger = logger;
        }



        [HttpPost("{participantId}")]
        public async Task<ActionResult> MapFSPs(int participantId, string cbdcAddress)
        {
            return Ok();
        }
    }
}
