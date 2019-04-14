using System;
using System.Collections.Generic;
using Firestorm.Data;
using JetBrains.Annotations;

namespace Firestorm.Fluent
{
    internal class DataSourceTypeFinder : IItemTypeFinder
    {
        private readonly IDataSource _dataSource;

        public DataSourceTypeFinder([NotNull] IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public IEnumerable<Type> FindItemTypes()
        {
            return _dataSource.FindEntityTypes();
        }
    }
}