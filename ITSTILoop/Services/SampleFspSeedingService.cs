﻿using System;
using ITSTILoop.Helpers;
using ITSTILoop.Model;
using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Services.Interfaces;
using CBDCHubContract.Services;
using ITSTILoopLibrary.DTO;

namespace ITSTILoop.Services
{
    public class SampleFspSeedingService : ISampleFspSeedingService
    {
        private readonly ILogger<SampleFspSeedingService> _logger;
        private readonly IParticipantRepository _participantRepository;
        private readonly ISettlementWindowRepository _settlementWindowRepository;
        private readonly CBDCHubService _cbdcBridgeService;

        public SampleFspSeedingService(ILogger<SampleFspSeedingService> logger, IParticipantRepository participantRepository, ISettlementWindowRepository settlementWindowRepository, CBDCHubService cBDCBridgeService)
        {
            _logger = logger;
            _participantRepository = participantRepository;
            _settlementWindowRepository = settlementWindowRepository;
            _cbdcBridgeService = cBDCBridgeService;
        }

        public Participant? CreateParticipant(string participantText)
        {
            var splitText = participantText.Split('|');
            if (splitText.Length == 4)
            {
                string name = splitText[0];
                string apiId = splitText[1];
                string apiKey = splitText[2];
                string cdbcAddress = splitText[3];                
                string lookupEndpoint = $"http://{name}/itstiloop/ITSTILoopPartyLookup";
                string transferEndpoint = $"http://{name}/itstiloop/ITSTILoopTransfer";
                var participant = _participantRepository.CreateParticipant(name, apiId, apiKey, new Uri(lookupEndpoint), new Uri(transferEndpoint), cdbcAddress);
                return participant;
            }
            return null;
        }

        public void CreateParties(int participantId, string partiesText)
        {
            var splitText = partiesText.Split('|');
            foreach (var partyIdentifier in splitText)
            {
                Party party = new Party() { ParticipantId = participantId, PartyIdentifierType = PartyIdTypes.MSISDN, PartyIdentifier = partyIdentifier };
                _participantRepository.AddPartyToParticipant(participantId, party);                
            }
        }

        public async Task SeedFspAsync(string participantText, string partiesText)
        {
            var participant = CreateParticipant(participantText);
            if (participant != null)
            {
                //TODO: if hub is connected to the cbdc
                decimal balance = 10000;
                //var balance = await _cbdcBridgeService.CheckBalanceAsync(participant.CBDCAddress);
                _logger.LogInformation($"Participant {participant.Name} {participant.CBDCAddress} balance = {balance}");
                participant.CreateAccount(ITSTILoopDTOLibrary.CurrencyCodes.USD);
                //TODO: Make this configurable
                participant.FundAccount(ITSTILoopDTOLibrary.CurrencyCodes.USD, balance);
                _participantRepository.Save();
                _settlementWindowRepository.UpdateSettlementWindow();
                CreateParties(participant.ParticipantId, partiesText);
            }
        }
    }
}

