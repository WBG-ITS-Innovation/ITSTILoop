using AutoMapper;
using ITSTILoop.Attributes;
using ITSTILoop.Context;
using ITSTILoop.DTO;
using ITSTILoop.Model.Interfaces;
using ITSTILoop.Model;
using Microsoft.AspNetCore.Mvc;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiKey]
    public class TransferController : ControllerBase
    {
        private readonly ILogger<TransferController> _logger;
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;

        public TransferController(ILogger<TransferController> logger, IParticipantRepository participantRepository, IMapper mapper)
        {
            _logger = logger;
            _participantRepository = participantRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult<TransferRequestResponseDTO> Post(TransferRequestDTO transferRequestDTO)
        {
            return Ok();
        }

        [HttpPut("{transferId}")]
        public ActionResult<TransferRequestCompleteDTO> Put(Guid transferId, [FromBody] TransferAcceptRejectDTO transferAcceptRejectDTO)
        {
            return Ok();
        }
    }
}