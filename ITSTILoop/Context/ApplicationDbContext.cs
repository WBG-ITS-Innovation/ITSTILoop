using ITSTILoop.Model;
using Microsoft.EntityFrameworkCore;

namespace ITSTILoop.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Participant> Participants => Set<Participant>();
        public DbSet<Party> Parties => Set<Party>();
        public DbSet<SettlementWindow> SettlementWindows => Set<SettlementWindow>();
    }
}
