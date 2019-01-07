using System;
using System.Linq;
using Firestorm.Testing.Data;
using Microsoft.EntityFrameworkCore;

namespace Firestorm.EntityFrameworkCore2.IntegrationTests
{
    public class ExampleFixture : IDisposable
    {
        public ExampleDataContext Context { get; }

        public ExampleFixture()
        {
            var options = new DbContextOptionsBuilder<ExampleDataContext>()
                .UseSqlServer(DbConnectionStrings.Resolve("Firestorm.EntityFrameworkCore2Tests"))
                .Options;
            
            Context = new ExampleDataContext(options);

            Context.Database.EnsureCreated();

            if (Context.Artists.Any()) 
                return;
                
            Context.Artists.AddRange(ExampleDataSets.GetArtists());
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}