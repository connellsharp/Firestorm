using Firestorm.Tests.Examples.Football.Models;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.Tests.Examples.Football.Data
{
    public class FootballDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(e =>
            {
                e.Property(t => t.Name);

                e.Property(t => t.FoundedYear);

                e.HasMany(t => t.Players).WithOne(p => p.Team);
            });

            modelBuilder.Entity<Player>(e =>
            {
                e.Property(p => p.Name);

                e.Property(p => p.SquadNumber);
            });
        }
    }
}
