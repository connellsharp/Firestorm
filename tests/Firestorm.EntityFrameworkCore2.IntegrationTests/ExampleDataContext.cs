using Firestorm.Testing.Models;
using Firestorm.Testing;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.EntityFrameworkCore2.IntegrationTests
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
            modelBuilder.Entity<Artist>().HasKey(a => a.ID);
            modelBuilder.Entity<Album>().HasKey(a => a.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}