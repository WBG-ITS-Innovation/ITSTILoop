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

        public SettlementController(ILogger<SettlementController> logger, ISettlementWindowRepository settlementWindowRepository)
        {
            _logger = logger;            
            _settlementWindowRepository = settlementWindowRepository;            
        }

        [HttpGet(Name = "GetSettlementWindows")]
        public IEnumerable<SettlementWindow> Get()
        {            
            return _settlementWindowRepository.GetAll();
        }
    }
}
