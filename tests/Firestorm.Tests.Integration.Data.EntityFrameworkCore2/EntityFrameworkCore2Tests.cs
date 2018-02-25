using System.Linq;
using Firestorm.EntityFrameworkCore2;
using Firestorm.Tests.Integration.Data.Base;
using Firestorm.Tests.Integration.Data.Base.Models;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.Tests.Integration.Data.EntityFrameworkCore2
{
    [UsedImplicitly]
    public class EntityFrameworkCore2Tests : BasicDataTests, IClassFixture<ExampleDataContext>
    {
        public EntityFrameworkCore2Tests(ExampleDataContext context) 
            : base(new EFCoreDataTransaction<ExampleDataContext>(context), new EFCoreRepository<Artist>(context.Artists))
        {
            context.Database.EnsureCreated();

            if (!context.Artists.Any())
            {
                context.Artists.AddRange(ExampleDataSets.GetArtists());
                context.SaveChanges();
            }
        }
    }
}
