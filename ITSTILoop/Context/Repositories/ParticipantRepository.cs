using ITSTILoop.Context.Repositories.Interfaces;
using ITSTILoop.Model;
using ITSTILoopDTOLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

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

        public Participant? GetByIdFull(int id)
        {
            return _context.Participants.Include(k => k.Accounts).Include(k => k.Parties).FirstOrDefault(k => k.ParticipantId == id);
        }

        public Participant? GetParticipantFromApiKey(string apiId, string apiKey)
        {
            return Find(k => k.ApiKey == apiKey && k.ApiId == apiId).FirstOrDefault();
        }

        public Participant? GetParticipantFromApiKeyId(IHeaderDictionary requestHeaders)
        {
            StringValues apiKey;
            StringValues apiId;
            if (requestHeaders.TryGetValue("ApiKey", out apiKey) && requestHeaders.TryGetValue("ApiId", out apiId))
            {
                return GetParticipantFromApiKey(apiId.First(), apiKey.First());
            }
            return null;
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

        public Participant CreateParticipant(string name, string apiId, string apiKey, Uri partyLookupEndpoint, Uri confirmTransferEndpoint)
        {
            Participant participant = new Participant() { Name = name, ApiId = apiId, ApiKey = apiKey, PartyLookupEndpoint = partyLookupEndpoint, ConfirmTransferEndpoint = confirmTransferEndpoint };
            _context.Participants.Add(participant);
            Save();
            return participant;
        }
    }
}
