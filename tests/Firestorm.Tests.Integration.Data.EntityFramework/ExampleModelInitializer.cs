using System.Collections.Generic;
using System.Data.Entity;
using Firestorm.Tests.Integration.Data.Base;
using Firestorm.Tests.Integration.Data.Base.Models;
using JetBrains.Annotations;

namespace Firestorm.Tests.Integration.Data.EntityFramework
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