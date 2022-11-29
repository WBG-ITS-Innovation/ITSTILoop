using ITSTILoopAddressLookup.Services;
using ITSTILoopLibrary.DTO;
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

        [HttpPost]
        public async Task<ActionResult<PartyDTO>> GetPartyAsync(PartyIdentifierDTO queryPartyDTO)
        {
            var partyQueryResult = _partyLookupService.FindParty(queryPartyDTO.PartyIdentifierType, queryPartyDTO.Identifier);
            if (partyQueryResult.Result == PartyLookupServiceResults.Success)
            {
                return Ok(partyQueryResult.FoundParty);
            }
            else
            {
                return NotFound(partyQueryResult.Result.ToString());
            }
        }

    }
}