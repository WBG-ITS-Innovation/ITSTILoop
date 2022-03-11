using ITSTILoop.Model;

namespace ITSTILoop.Model.Interfaces
{
    public interface IParticipantRepository : IGenericRepository<Participant>
    {
        Participant? GetParticipantFromApiKey(string apiId, string apiKey);
        void AddPartyToParticipant(int participantId, Party party);
        Participant? GetParticipantByName(string name);
    }
}
