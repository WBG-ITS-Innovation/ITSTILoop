using ITSTILoopLibrary.DTO;
using ITSTILoopLibrary.SampleFSPServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITSTILoopCBDCAdapter.Controllers
{

    [Route("itstiloop/[controller]")]
    [ApiController]
    public class ITSTILoopPartyLookupController : ControllerBase
    {       
        private readonly ILogger<ITSTILoopPartyLookupController> _logger;        

        public ITSTILoopPartyLookupController(ILogger<ITSTILoopPartyLookupController> logger)
        {
            _logger = logger;            
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PartyDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost(Name = "QueryParty")]
        public ActionResult<PartyDTO> Post(PartyIdentifierDTO partyIdentifierDTO)
        {
            //var account = AccountService.Accounts.FirstOrDefault(k => k.PartyDefinition.PartyIdentifier.Identifier == partyIdentifierDTO.Identifier && k.PartyDefinition.PartyIdentifier.PartyIdentifierType == partyIdentifierDTO.PartyIdentifierType);
            //if (account != null)
            //{
            //    return account.PartyDefinition;
            //}            
            return NotFound();            
        }
    }
}
