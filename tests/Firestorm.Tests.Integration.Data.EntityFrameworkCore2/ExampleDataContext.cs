using Firestorm.Tests.Integration.Data.Base.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.Tests.Integration.Data.EntityFrameworkCore2
{
    [UsedImplicitly]
    public class ExampleDataContext : DbContext
    {
        public ExampleDataContext() 
            : base(GetOptions())
        { }

        private static DbContextOptions GetOptions()
        {
            return new DbContextOptionsBuilder<ExampleDataContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Firestorm.Tests.Integration.EntityFramework6.ExampleDataContext;integrated security=True;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;
        }

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