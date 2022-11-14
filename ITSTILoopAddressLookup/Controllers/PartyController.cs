using ITSTILoopAddressLookup.Services;
using ITSTILoopDTOLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace ITSTILoopAddressLookup.Controllers
{
    [ApiController]
    [Route("[controller]")]    
    public class PartyController : ControllerBase
    {
        private readonly ILogger<PartyController> _logger;
        private readonly PartyLookupService _partyLookupService;

        public PartyController(ILogger<PartyController> logger, PartyLookupService partyLookupService)
        {
            _logger = logger;
            _partyLookupService = partyLookupService;
        }

        [HttpGet]
        public async Task<ActionResult<GlobalPartyIdentifierDTO>> GetPartyAsync(PartyIdTypes partyIdType, string partyIdentifier)
        {
            var partyQueryResult = _partyLookupService.FindParty(partyIdType, partyIdentifier);
            if (partyQueryResult.Result == PartyLookupServiceResults.Success)
            {
                return Ok(partyQueryResult.FoundParty);
            }
            else
            {
                return Problem(partyQueryResult.Result.ToString());
            }
        }

    }
}