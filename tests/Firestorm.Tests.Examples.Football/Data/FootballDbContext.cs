using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace Firestorm.Tests.Examples.Football.Models
{
    public class FootballDbContext : DbContext
    {
        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[] { new DebugLoggerProvider((_, __) => true) });

        public FootballDbContext(DbContextOptions<FootballDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

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

                e.HasMany(p => p.Fixtures).WithOne(t => t.Team).HasForeignKey(ft => ft.TeamId);
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

            modelBuilder.Entity<FixtureTeam>(e =>
            {
                e.HasKey(p => new { p.FixtureId, p.IsHome });

                e.Property(p => p.IsHome);
            });

            modelBuilder.Entity<Fixture>(e =>
            {
                e.HasKey(p => p.Id);

                e.HasMany(f => f.Goals).WithOne(g => g.Fixture);

                e.HasMany(p => p.Teams).WithOne(t => t.Fixture).HasForeignKey(ft => ft.FixtureId);
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
