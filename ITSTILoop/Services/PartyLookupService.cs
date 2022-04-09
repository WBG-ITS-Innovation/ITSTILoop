using AutoMapper;
using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Services.Interfaces;
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Services
{
    public class PartyLookupService : IPartyLookupService
    {
        private readonly ILogger<PartyLookupService> _logger;
        private readonly IParticipantRepository _participantRepository;
        private readonly IPartyRepository _partyRepository;
        private readonly IHttpPostClient _httpPostClient;        

        public PartyLookupService(ILogger<PartyLookupService> logger, IParticipantRepository participantRepository, IPartyRepository partyRepository, IHttpPostClient httpPostClient)
        {
            _logger = logger;
            _participantRepository = participantRepository;
            _partyRepository = partyRepository;
            _httpPostClient = httpPostClient;

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
                    var postResponse = await _httpPostClient.PostAsync<PartyIdentifierDTO, PartyDTO>(queryPartyDTO, participant.PartyLookupEndpoint);
                    if (postResponse.Result == HttpPostClientResults.Success)
                    {
                        result.FoundParty = postResponse.ResponseContent;
                    }
                    else
                    {
                        result.Result = PartyLookupServiceResults.EndpointError;
                    }
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
