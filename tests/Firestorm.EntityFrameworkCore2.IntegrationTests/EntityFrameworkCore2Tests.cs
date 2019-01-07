using Firestorm.Testing.Data;
using Firestorm.Testing.Models;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.EntityFrameworkCore2.IntegrationTests
{
    [UsedImplicitly]
    public class EntityFrameworkCore2Tests : BasicDataTests, IClassFixture<ExampleFixture>
    {
        public EntityFrameworkCore2Tests(ExampleFixture fixture)
            : base(
                new EFCoreDataTransaction<ExampleDataContext>(fixture.Context),
                new EFCoreRepository<Artist>(fixture.Context.Artists)
            )
        {
        }
    }
}
