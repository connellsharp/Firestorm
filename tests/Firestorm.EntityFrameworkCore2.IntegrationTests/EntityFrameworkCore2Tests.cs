using System.Linq;
using Firestorm.EntityFrameworkCore2;
using Firestorm.Tests.Integration.Data.Base;
using Firestorm.Tests.Integration.Data.Base.Models;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.Tests.Integration.Data.EntityFrameworkCore2
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
