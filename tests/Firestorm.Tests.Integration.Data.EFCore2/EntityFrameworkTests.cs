using Firestorm.Data.EFCore2;
using Firestorm.Tests.Integration.Data.Base;
using Firestorm.Tests.Integration.Data.Base.Models;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.Tests.Integration.Data.EFCore2
{
    [UsedImplicitly]
    public class EntityFrameworkTests : BasicDataTests, IClassFixture<ExampleDataContext>
    {
        public EntityFrameworkTests(ExampleDataContext context) 
            : base(new EFCoreDataTransaction<ExampleDataContext>(context), new EFCoreRepository<Artist>(context.Artists))
        {
        }
    }
}
