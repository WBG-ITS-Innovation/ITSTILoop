using ITSTILoop.Model.Interfaces;
using ITSTILoop.Model;
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
        }

        public Participant? GetParticipantFromApiKey(string apiId, string apiKey)
        {
            return Find(k => k.ApiKey == apiKey && k.Name == apiId).FirstOrDefault();
        }

        public Participant? GetParticipantByName(string name)
        {
            return Find(k => k.Name == name).FirstOrDefault();
        }
    }
}
