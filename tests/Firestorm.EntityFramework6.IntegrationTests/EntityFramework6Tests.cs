using Firestorm.Testing.Data;
using Firestorm.Testing.Models;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.EntityFramework6.IntegrationTests
{
    [UsedImplicitly]
    public class EntityFramework6Tests : BasicDataTests, IClassFixture<EntitiesDataTransaction<ExampleDataContext>>
    {
        public EntityFramework6Tests(EntitiesDataTransaction<ExampleDataContext> transaction) 
            : base(transaction, new EntitiesRepository<Artist>(transaction.DbContext.Artists))
        {
        }
    }
}
