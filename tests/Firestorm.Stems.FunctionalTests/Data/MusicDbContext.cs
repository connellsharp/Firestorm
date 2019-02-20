using Firestorm.Stems.FunctionalTests.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace Firestorm.Stems.FunctionalTests.Data
{
    [UsedImplicitly]
    public class MusicDbContext : DbContext
    {
        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[] { new DebugLoggerProvider((_, __) => true) });

        public MusicDbContext(DbContextOptions<MusicDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

        public virtual DbSet<Artist> Artists { get; set; }

        public virtual DbSet<Album> Albums { get; set; }

        public virtual DbSet<Track> Tracks { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<LikedTrack> LikedTracks { get; set; }
    }
}