using System.Data.Entity;
using Firestorm.Testing.Models;
using Firestorm.Tests.Integration.Data.Base;
using Firestorm.Testing;
using JetBrains.Annotations;

namespace Firestorm.EntityFramework6.IntegrationTests
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
            modelBuilder.Entity<Artist>().HasKey(a => a.ID);
            modelBuilder.Entity<Album>().HasKey(a => a.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}