using AutoMapper;
using ITSTILoop.Attributes;
using ITSTILoop.Context;
using ITSTILoop.DTO;
using ITSTILoop.Model.Interfaces;
using ITSTILoop.Model;
using Microsoft.AspNetCore.Mvc;

namespace ITSTILoop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiKey]
    public class ParticipantController : ControllerBase
    {
        private readonly ILogger<ParticipantController> _logger;
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;

        public ParticipantController(ILogger<ParticipantController> logger, IParticipantRepository participantRepository, IMapper mapper)
        {
            _logger = logger;
            _participantRepository = participantRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetParticipants")]
        public IEnumerable<ParticipantDTO> Get()
        {
            //TODO: add to interface
            return _mapper.Map<IEnumerable<Participant>, IEnumerable<ParticipantDTO>>(_participantRepository.GetAll());            
        }

        [HttpGet("{id}")]
        public ActionResult<ParticipantDTO> GetParticipant(int id)
        {
            var participant = _participantRepository.GetById(id);

            if (participant == null)
            {
                return NotFound();
            }
            return _mapper.Map<ParticipantDTO>(participant);            
        }

        [HttpPost]
        public ActionResult<ParticipantDTO> Post(RegisterParticipantDTO registerParticipantDTO)
        {            
            var participant = _participantRepository.CreateParticipant(registerParticipantDTO.Name, registerParticipantDTO.ApiKey, registerParticipantDTO.PartyLookupEndpoint);            
            var participantDto = _mapper.Map<ParticipantDTO>(participant);
            return CreatedAtAction("GetParticipant", new { id = participant.ParticipantId }, participantDto);
        }

        [Route("Fund")]
        [HttpPost]
        public ActionResult FundParticipant([FromBody] FundParticipantDTO fundParticipantDTO)
        {
            _participantRepository.FundParticipant(fundParticipantDTO.ParticipantId, fundParticipantDTO.Currency, fundParticipantDTO.Amount);
            return Ok();
        }
    }
}