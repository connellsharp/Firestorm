using System;
using System.Collections.Generic;
using Firestorm.Data;
using JetBrains.Annotations;

namespace Firestorm.Extensions.AspNetCore
{
    internal class DataSourceTypeFinder : IItemTypeFinder
    {
        private readonly IDiscoverableDataSource _discoverableDataSource;

        public DataSourceTypeFinder([NotNull] IDataSource dataSource)
        {
            if (!(dataSource is IDiscoverableDataSource discoverableDataSource))
                throw new ArgumentException("Data source cannot automatically find the item types.", nameof(dataSource));
            
            _discoverableDataSource = discoverableDataSource;
        }

        public IEnumerable<Type> FindItemTypes()
        {
            return _discoverableDataSource.FindRepositoryTypes();
        }
    }
}