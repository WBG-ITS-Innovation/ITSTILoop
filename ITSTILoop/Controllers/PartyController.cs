using AutoMapper;
using ITSTILoop.Attributes;
using ITSTILoop.Model;
using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Services;
using ITSTILoop.Services.Interfaces;
using ITSTILoopDTOLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace ITSTILoop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiKey]
    public class PartyController : ControllerBase
    {
        private readonly ILogger<PartyController> _logger;
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;
        private readonly IPartyLookupService _partyLookupService;

        public PartyController(ILogger<PartyController> logger, IParticipantRepository particpantRepository, IMapper mapper, IPartyLookupService partyLookupService )
        {
            _logger = logger;            
            _participantRepository = particpantRepository;
            _mapper = mapper;
            _partyLookupService = partyLookupService;
        }

        [HttpGet]
        public async Task<ActionResult<PartyDTO>> GetPartyAsync(PartyIdTypes partyIdType, string partyIdentifier)
        {
            var partyQueryResult = await _partyLookupService.FindPartyAsync(partyIdType, partyIdentifier);
            if (partyQueryResult.Result == PartyLookupServiceResults.Success)
            {
                return partyQueryResult.FoundParty;
            }
            else
            {
                return Problem(partyQueryResult.Result.ToString());
            }
        }

        [HttpPost]
        public ActionResult<PartyDTO> Post(PartyIdentifierDTO registerPartyDTO)
        {
            StringValues apiKey;
            StringValues apiId;
            if (Request.Headers.TryGetValue("ApiKey", out apiKey) && Request.Headers.TryGetValue("ApiId", out apiId))
            {
                var participant = _participantRepository.GetParticipantFromApiKey(apiId.First(), apiKey.First());
                if (participant != null)
                {
                    var party = _mapper.Map<Party>(registerPartyDTO);
                    party.ParticipantId = participant.ParticipantId;
                    _participantRepository.AddPartyToParticipant(participant.ParticipantId, party);                    
                    var partyDto = _mapper.Map<PartyDTO>(party);
                    return CreatedAtAction("GetParty", null);

                }
                else
                {
                    return Problem("Participant Not Found");
                }
            }
            else
            {
                return BadRequest("ApiKey doesn't match any participant");
            }
        }
    }
}