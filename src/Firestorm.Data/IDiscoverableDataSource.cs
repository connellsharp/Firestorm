using System;
using System.Collections.Generic;

namespace Firestorm.Data
{
    /// <summary>
    /// An <see cref="IDataSource"/> that exposes a list of types that can be used for repositories.
    /// </summary>
    public interface IDiscoverableDataSource : IDataSource
    {
        IEnumerable<Type> FindRepositoryTypes();
    }
}