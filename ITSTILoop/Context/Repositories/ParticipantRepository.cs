using ITSTILoop.Model;
using ITSTILoop.Model.Interfaces;
using ITSTILoopDTOLibrary;
using Microsoft.EntityFrameworkCore;

namespace ITSTILoop.Context.Repositories
{
    public class ParticipantRepository : GenericRepository<Participant>, IParticipantRepository
    {
        public ParticipantRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void AddPartyToParticipant(int participantId, Party party)
        {
            var participant = _context.Participants.Include(k => k.Parties).FirstOrDefault(k => k.ParticipantId == participantId);
            if (participant != null)
            {
                participant.Parties.Add(party);
            }
            Save();
        }

        public Participant? GetParticipantFromApiKey(string apiId, string apiKey)
        {
            return Find(k => k.ApiKey == apiKey && k.Name == apiId).FirstOrDefault();
        }

        public Participant? GetParticipantByName(string name)
        {
            return Find(k => k.Name == name).FirstOrDefault();
        }

        public void FundParticipant(int participantId, CurrencyCodes currency, decimal amount)
        {
            var participant = GetById(participantId);
            if (participant != null)
            {
                participant.FundAccount(currency, amount);
                Save();
            }
        }

        public Participant CreateParticipant(string name, string apiKey, Uri partyLookupEndpoint)
        {
            Participant participant = new Participant() { Name = name, ApiKey = apiKey, PartyLookupEndpoint = partyLookupEndpoint };
            _context.Participants.Add(participant);
            Save();
            return participant;
        }
    }
}
