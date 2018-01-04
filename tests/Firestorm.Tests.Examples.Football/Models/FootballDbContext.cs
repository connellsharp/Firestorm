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
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<League> Leagues { get; set; }

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

            modelBuilder.Entity<League>(e =>
            {
                e.HasKey(p => p.Id);

                e.Property(p => p.Name);

                e.HasMany(p => p.Teams).WithOne(t => t.League);
            });

            modelBuilder.Entity<Fixture>(e =>
            {
                e.HasKey(p => p.Id);

                e.HasOne(p => p.AwayTeam).WithMany(t => t.AwayFixtures);

                e.HasOne(p => p.HomeTeam).WithMany(t => t.HomeFixtures);

                e.HasMany(f => f.Goals).WithOne(g => g.Fixture);
            });

            modelBuilder.Entity<Goal>(e =>
            {
                e.HasKey(p => p.Id);

                e.HasOne(p => p.Player).WithMany(t => t.Goals);

                e.HasOne(p => p.Fixture).WithMany(t => t.Goals);
            });
        }
    }
}
