using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Firestorm.Stems.FunctionalTests.Data.Models;
using Firestorm.Testing.Data;
using JetBrains.Annotations;

namespace Firestorm.Stems.FunctionalTests.Data
{
    [UsedImplicitly]
    public class ExampleDataContext : DbContext
    {
        public ExampleDataContext()
            : base(DbConnectionStrings.Resolve("Firestorm.MusicExample"))
        { }

        public virtual DbSet<Artist> Artists { get; set; }

        public virtual DbSet<Album> Albums { get; set; }

        public virtual DbSet<Track> Tracks { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<LikedTrack> LikedTracks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}