using AutoMapper;
using ITSTILoop.Attributes;
using ITSTILoop.Model;
using ITSTILoop.Model.Interfaces;
using ITSTILoop.Services;
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
        private readonly IPartyRepository _partyRepository;
        private readonly IMapper _mapper;
        private readonly IPartyLookupService _partyLookupService;

        public PartyController(ILogger<PartyController> logger, IParticipantRepository particpantRepository, IPartyRepository partyRepository, IMapper mapper, IPartyLookupService partyLookupService )
        {
            _logger = logger;            
            _participantRepository = particpantRepository;
            _partyRepository = partyRepository;
            _mapper = mapper;
            _partyLookupService = partyLookupService;
        }

        [HttpGet]
        public async Task<ActionResult<PartyDTO>> GetPartyAsync(PartyIdTypes partyIdType, string partyIdentifier)
        {
            var party = _partyRepository.GetPartyFromTypeAndId(partyIdType, partyIdentifier);            
            if (party != null)
            {
                var participant = _participantRepository.GetParticipantByName(party.RegisteredParticipantName);
                if (participant != null)
                {
                    QueryPartyDTO queryPartyDTO = new QueryPartyDTO() { PartyIdentifier = partyIdentifier, PartyIdentifierType = partyIdType };
                    var result = await _partyLookupService.LookupPartyAsync(queryPartyDTO, participant.PartyLookupEndpoint);
                    if (result.Result == PartyLookupServiceResults.Success)
                    {
                        return result.FoundParty;
                    }
                    else
                    {
                        return Problem("Participant Endpoint Call Error");
                    }
                }
                else
                {
                    return NotFound("Participant Not Registered");
                }
            }
            else
            {
                return NotFound("Party Not Registered");
            }            
        }

        [HttpPost]
        public ActionResult<PartyDTO> Post(RegisterPartyDTO registerPartyDTO)
        {
            StringValues apiKey;
            StringValues apiId;
            if (Request.Headers.TryGetValue("ApiKey", out apiKey) && Request.Headers.TryGetValue("ApiId", out apiId))
            {
                var participant = _participantRepository.GetParticipantFromApiKey(apiId.First(), apiKey.First());
                if (participant != null)
                {
                    var party = _mapper.Map<Party>(registerPartyDTO);
                    party.RegisteredParticipantName = participant.Name;
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