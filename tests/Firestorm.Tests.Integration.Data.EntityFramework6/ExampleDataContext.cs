using System.Data.Entity;
using Firestorm.Tests.Integration.Data.Base.Models;
using JetBrains.Annotations;

namespace Firestorm.Tests.Integration.Data.EntityFramework6
{
    [UsedImplicitly]
    public class ExampleDataContext : DbContext
    {
        // Your context has been configured to use a 'ExampleDataContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Firestorm.Tests.Examples.Music.ExampleDataContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ExampleDataContext' 
        // connection string in the application configuration file.
        public ExampleDataContext()
            : base("name=ExampleDataContext")
        { }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

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