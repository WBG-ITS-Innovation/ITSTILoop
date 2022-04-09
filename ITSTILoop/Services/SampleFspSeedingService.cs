using System;
using ITSTILoop.Helpers;
using ITSTILoop.Model;
using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Services.Interfaces;

namespace ITSTILoop.Services
{
    public class SampleFspSeedingService : ISampleFspSeedingService
    {
        private readonly ILogger<SampleFspSeedingService> _logger;
        private readonly IParticipantRepository _participantRepository;
        private readonly ISettlementWindowRepository _settlementWindowRepository;

        public SampleFspSeedingService(ILogger<SampleFspSeedingService> logger, IParticipantRepository participantRepository, ISettlementWindowRepository settlementWindowRepository)
        {
            _logger = logger;
            _participantRepository = participantRepository;
            _settlementWindowRepository = settlementWindowRepository;
        }

        public Participant? CreateParticipant(string participantText)
        {
            var splitText = participantText.Split('|');
            if (splitText.Length == 3)
            {
                string name = splitText[0];
                string apiId = splitText[1];
                string apiKey = splitText[2];
                string lookupEndpoint = $"http://{name}/itsti/itstilooppartylookup";
                string transferEndpoint = $"http://{name}/itsti/itstilooptransfer";
                return _participantRepository.CreateParticipant(name, apiId, apiKey, new Uri(lookupEndpoint), new Uri(transferEndpoint));
            }
            return null;
        }

        public void CreateParties(int participantId, string partiesText)
        {
            var splitText = partiesText.Split('|');
            foreach (var partyIdentifier in splitText)
            {
                Party party = new Party() { ParticipantId = participantId, PartyIdentifierType = ITSTILoopDTOLibrary.PartyIdTypes.MSISDN, PartyIdentifier = partyIdentifier };
                _participantRepository.AddPartyToParticipant(participantId, party);                
            }
        }

        public void SeedFsp(string participantText, string partiesText)
        {
            var participant = CreateParticipant(participantText);
            if (participant != null)
            {
                _participantRepository.FundParticipant(participant.ParticipantId, ITSTILoopDTOLibrary.CurrencyCodes.USD, 50000);
                _settlementWindowRepository.UpdateSettlementWindow();
                CreateParties(participant.ParticipantId, partiesText);
            }
        }
    }
}

