﻿using Firestorm.EntityFramework6;
using Firestorm.Testing.Data;
using Firestorm.Testing.Data.Models;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.Tests.Integration.Data.EntityFramework6
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
