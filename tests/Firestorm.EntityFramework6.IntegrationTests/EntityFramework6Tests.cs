using Firestorm.Testing.Data;
using Firestorm.Testing.Models;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.EntityFramework6.IntegrationTests
{
    [UsedImplicitly]
    public class EntityFramework6Tests : BasicDataTests, IClassFixture<ExampleFixture>
    {
        public EntityFramework6Tests(ExampleFixture fixture) 
            : base(
                new EntitiesDataTransaction<ExampleDataContext>(fixture.Context),
                new EntitiesRepository<Artist>(fixture.Context.Artists))
        {
        }
    }
}
