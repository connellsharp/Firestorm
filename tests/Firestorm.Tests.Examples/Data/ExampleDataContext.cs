using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Firestorm.Tests.Examples.Data.Models;
using JetBrains.Annotations;

namespace Firestorm.Tests.Examples.Data
{
    [UsedImplicitly]
    public class ExampleDataContext : DbContext
    {
        // Your context has been configured to use a 'ExampleDataContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Firestorm.Tests.Examples.ExampleDataContext' database on your LocalDb instance. 
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

        public virtual DbSet<Track> Tracks { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<LikedTrack> LikedTracks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}