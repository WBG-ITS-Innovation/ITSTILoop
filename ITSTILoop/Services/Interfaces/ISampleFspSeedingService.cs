namespace ITSTILoop.Services.Interfaces
{
    public interface ISampleFspSeedingService
    {        
        Task SeedFspAsync(string participantText, string partiesText);
    }
}