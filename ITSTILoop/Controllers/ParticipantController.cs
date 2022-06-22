using AutoMapper;
using ITSTILoop.Attributes;
using ITSTILoop.Context;
using ITSTILoop.DTO;
using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Model;
using Microsoft.AspNetCore.Mvc;
using ITSTILoop.Context.Repositories;

namespace ITSTILoop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiKey]
    public class ParticipantController : ControllerBase
    {
        private readonly ILogger<ParticipantController> _logger;
        private readonly IParticipantRepository _participantRepository;
        private readonly ISettlementWindowRepository _settlementWindowRepository;
        private readonly IMapper _mapper;

        public ParticipantController(ILogger<ParticipantController> logger, IParticipantRepository participantRepository, IMapper mapper, ISettlementWindowRepository settlementWindowRepository)
        {
            _logger = logger;
            _participantRepository = participantRepository;
            _settlementWindowRepository = settlementWindowRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetParticipants")]
        public IEnumerable<ParticipantDTO> Get()
        {
            //TODO: add to interface
            var allParticipants = _participantRepository.GetAll();
            return _mapper.Map<IEnumerable<Participant>, IEnumerable<ParticipantDTO>>(allParticipants);            
        }

        [HttpGet("{id}")]
        public ActionResult<Participant> GetParticipant(int id)
        {
            var participant = _participantRepository.GetByIdFull(id);

            if (participant == null)
            {
                return NotFound();
            }
            return participant;            
        }

        [HttpPost]
        public ActionResult<ParticipantDTO> Post(RegisterParticipantDTO registerParticipantDTO)
        {            
            var participant = _participantRepository.CreateParticipant(registerParticipantDTO.Name, registerParticipantDTO.ApiId, registerParticipantDTO.ApiKey, registerParticipantDTO.PartyLookupEndpoint, registerParticipantDTO.ConfirmTransferEndpoint,"");            
            var participantDto = _mapper.Map<ParticipantDTO>(participant);
            return CreatedAtAction("GetParticipant", new { id = participant.ParticipantId }, participantDto);
        }

        [Route("Fund")]
        [HttpPost]
        public ActionResult FundParticipant([FromBody] FundParticipantDTO fundParticipantDTO)
        {
            _participantRepository.FundParticipant(fundParticipantDTO.ParticipantId, fundParticipantDTO.Currency, fundParticipantDTO.Amount);
            _settlementWindowRepository.UpdateSettlementWindow();
            return Ok();
        }

        [Route("Modify")]
        [HttpPost]
        public ActionResult ModifyParticipant([FromBody] ModifyParticipantDTO modifyParticipantDTO)
        {
            _participantRepository.ModifyParticipant(modifyParticipantDTO.ParticipantId, modifyParticipantDTO.Currency, modifyParticipantDTO.Position, modifyParticipantDTO.NetSettlement);
            _settlementWindowRepository.UpdateSettlementWindow();
            return Ok();
        }
    }
}