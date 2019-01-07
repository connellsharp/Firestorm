using System.Collections.Generic;
using System.Data.Entity;
using Firestorm.Testing.Data;
using Firestorm.Tests.Unit;
using JetBrains.Annotations;

namespace Firestorm.EntityFramework6.IntegrationTests
{
    [UsedImplicitly]
    public class ExampleModelInitializer : DropCreateDatabaseAlways<ExampleDataContext>
    {
        protected override void Seed(ExampleDataContext context)
        {
            context.Artists.AddRange(ExampleDataSets.GetArtists());

            context.Albums.AddRange(new List<Album>
            {
            });

            context.SaveChanges();
        }
    }
}