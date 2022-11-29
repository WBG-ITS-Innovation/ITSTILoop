using ITSTILoopDTOLibrary;
using ITSTILoopLibrary.UtilityServices.Interfaces;
using ITSTILoopSampleFSP.Models;
using ITSTILoopSampleFSP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITSTILoopSampleFSP.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
      

        private readonly ILogger<AccountsController> _logger;
        private readonly IHttpPostClient _httpPostClient;
        private readonly AccountService _accountService;

        public AccountsController(ILogger<AccountsController> logger, IHttpPostClient httpPostClient, AccountService accountService)
        {
            _logger = logger;
            _httpPostClient = httpPostClient;
            _accountService = accountService;
        }

        [HttpGet]
        public IEnumerable<FspAccount> Get()
        {
            return _accountService.Accounts;
        }

        [HttpPost("{partyIdentifier}")]
        public ActionResult DepositAccount(string partyIdentifier, decimal amount)
        {
            var account = _accountService.Accounts.FirstOrDefault(k => k.PartyDefinition.PartyIdentifier.Identifier == partyIdentifier);
            if (account != null)
            {
                account.Deposit(amount);
            }
            return NotFound("Account Not Found");
        }


    }
}
