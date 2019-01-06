using Firestorm.Tests.Integration.Data.Base.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.Tests.Integration.Data.EntityFrameworkCore2
{
    [UsedImplicitly]
    public class ExampleDataContext : DbContext
    {
        public ExampleDataContext(DbContextOptions options) 
            : base(options)
        { }

        public virtual DbSet<Artist> Artists { get; set; }

        public virtual DbSet<Album> Albums { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>().HasKey(a => a.ArtistID);
            modelBuilder.Entity<Album>().HasKey(a => a.AlbumID);

            base.OnModelCreating(modelBuilder);
        }
    }
}