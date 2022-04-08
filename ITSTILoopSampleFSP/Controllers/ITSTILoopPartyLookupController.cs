using ITSTILoopDTOLibrary;
using ITSTILoopSampleFSP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITSTILoopSampleFSP.Controllers
{

    [Route("itstiloop/[controller]")]
    [ApiController]
    public class ITSTILoopPartyLookupController : ControllerBase
    {       
        private readonly ILogger<ITSTILoopPartyLookupController> _logger;
        private readonly AccountService _accountService;

        public ITSTILoopPartyLookupController(ILogger<ITSTILoopPartyLookupController> logger, AccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PartyDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost(Name = "QueryParty")]
        public ActionResult<PartyDTO> Post(PartyIdentifierDTO partyIdentifierDTO)
        {
            var account = _accountService.Accounts.FirstOrDefault(k => k.PartyDefinition.PartyIdentifier.Identifier == partyIdentifierDTO.Identifier && k.PartyDefinition.PartyIdentifier.PartyIdentifierType == partyIdentifierDTO.PartyIdentifierType);
            if (account != null)
            {
                return account.PartyDefinition;
            }            
            return NotFound();            
        }
    }
}
