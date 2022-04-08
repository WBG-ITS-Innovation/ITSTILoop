using AutoMapper;
using ITSTILoop.Model.Interfaces;
using ITSTILoop.Services.Interfaces;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Services
{
    public class PartyLookupService : IPartyLookupService
    {
        private readonly ILogger<PartyLookupService> _logger;
        private readonly IParticipantRepository _participantRepository;
        private readonly IPartyRepository _partyRepository;
        private readonly IParticipantPartyQueryService _partyLookupService;

        public PartyLookupService(ILogger<PartyLookupService> logger, IParticipantRepository participantRepository, IPartyRepository partyRepository, IParticipantPartyQueryService partyLookupService)
        {
            _logger = logger;
            _participantRepository = participantRepository;
            _partyRepository = partyRepository;
            _partyLookupService = partyLookupService;
        }
        public async Task<PartyLookupServiceResult> FindPartyAsync(PartyIdTypes partyIdType, string partyIdentifier)
        {
            PartyLookupServiceResult result = new PartyLookupServiceResult();
            var party = _partyRepository.GetPartyFromTypeAndId(partyIdType, partyIdentifier);
            if (party != null)
            {
                var participant = _participantRepository.GetById(party.ParticipantId);
                if (participant != null)
                {
                    PartyIdentifierDTO queryPartyDTO = new PartyIdentifierDTO() { Identifier = partyIdentifier, PartyIdentifierType = partyIdType };
                    result = await _partyLookupService.LookupPartyAsync(queryPartyDTO, participant.PartyLookupEndpoint);
                }
                else
                {
                    result.Result = PartyLookupServiceResults.ParticipantNotRegistered;
                }
            }
            else
            {
                result.Result = PartyLookupServiceResults.PartyNotFound;
            }
            return result;
        }
    }
}
