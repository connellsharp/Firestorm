using System.Collections.Generic;
using System.Data.Entity;
using Firestorm.Testing.Data;
using Firestorm.Testing.Data.Models;
using JetBrains.Annotations;

namespace Firestorm.Tests.Integration.Data.EntityFramework6
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