using CBDCHubContract.Services;
using ITSTILoop.Attributes;
using ITSTILoop.Context.Repositories;
using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Model;
using ITSTILoop.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITSTILoop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiKey]
    public class SettlementController : ControllerBase
    {
        private readonly ILogger<SettlementController> _logger;
        private readonly ISettlementWindowRepository _settlementWindowRepository;
        private readonly CBDCHubService _cbdcBridgeService;

        public SettlementController(ILogger<SettlementController> logger, ISettlementWindowRepository settlementWindowRepository, CBDCHubService cBDCBridgeService)
        {
            _logger = logger;
            _settlementWindowRepository = settlementWindowRepository;
            _cbdcBridgeService = cBDCBridgeService;
        }

        [HttpGet(Name = "GetSettlementWindows")]
        public IEnumerable<SettlementWindow> Get()
        {
            return _settlementWindowRepository.GetAll();
        }

        [HttpPost("{settlementId}")]
        public async Task<ActionResult> Post(int settlementId)
        {
            //initiate the settlement
            var netSettlementDictionary = _settlementWindowRepository.GetNetSettlementDictionary(settlementId);
            //TODO: We need some error/exception checking here
            await _cbdcBridgeService.SettleAccountsAsync(settlementId, netSettlementDictionary);
            return Ok();
        }

    }
}
