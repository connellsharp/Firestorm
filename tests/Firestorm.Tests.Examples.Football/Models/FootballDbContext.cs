using Microsoft.EntityFrameworkCore;

namespace Firestorm.Tests.Examples.Football.Models
{
    public class FootballDbContext : DbContext
    {
        public FootballDbContext(DbContextOptions<FootballDbContext> options)
            : base(options)
        { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(e =>
            {
                e.HasKey(t => t.Id);

                e.Property(t => t.Name);

                e.Property(t => t.FoundedYear);

                e.HasMany(t => t.Players).WithOne(p => p.Team);
            });

            modelBuilder.Entity<Player>(e =>
            {
                e.HasKey(p => p.Id);

                e.Property(p => p.Name);

                e.Property(p => p.SquadNumber);
            });
        }
    }
}
