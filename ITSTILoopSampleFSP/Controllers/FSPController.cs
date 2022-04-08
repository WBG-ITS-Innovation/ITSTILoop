using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITSTILoopSampleFSP.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FSPController : ControllerBase
    {
      

        private readonly ILogger<FSPController> _logger;

        public FSPController(ILogger<FSPController> logger)
        {
            _logger = logger;
        }



        [HttpGet(Name = "Health")]
        public ActionResult<bool> Get()
        {
            return true;
        }
    }
}
