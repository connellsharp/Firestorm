using System;
using System.Collections.Generic;

namespace Firestorm.Data
{
    /// <summary>
    /// Provides a simple source interface to create data transactions and get repositories used in the Engine.
    /// </summary>
    /// <remarks>
    /// There will usually be one instance of this per application.
    /// </remarks>
    public interface IDataSource
    {
        /// <summary>
        /// Finds all entity types that can be used in <see cref="CreateContext{TEntity}"/>.
        /// </summary>
        IEnumerable<Type> FindEntityTypes();
        
        /// <summary>
        /// Creates a new <see cref="IDataContext{TEntity}"/> for a new request.
        /// </summary>
        IDataContext<TEntity> CreateContext<TEntity>()
            where TEntity : class, new();
    }
}