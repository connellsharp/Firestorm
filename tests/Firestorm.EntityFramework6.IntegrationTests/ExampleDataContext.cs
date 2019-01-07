using System.Data.Entity;
using Firestorm.Testing.Data;
using Firestorm.Testing.Data.Models;
using JetBrains.Annotations;

namespace Firestorm.Tests.Integration.Data.EntityFramework6
{
    [UsedImplicitly]
    public class ExampleDataContext : DbContext
    {
        public ExampleDataContext()
            : base(DbConnectionStrings.Resolve("Firestorm.EntityFramework6Tests"))
        { }

        public virtual DbSet<Artist> Artists { get; set; }

        public virtual DbSet<Album> Albums { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>().HasKey(a => a.ArtistID);
            modelBuilder.Entity<Album>().HasKey(a => a.AlbumID);

            base.OnModelCreating(modelBuilder);
        }
    }
}