namespace Firestorm.Data
{
    /// <summary>
    /// Contains the services used to manage entities in a database.
    /// </summary>
    public interface IDataContext<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Manages the data transaction lifetime for an <see cref="IEngineRepository{TItem}"/>.
        /// </summary>
        IDataTransaction Transaction { get; }
        
        /// <summary>
        /// Manages data storage.
        /// </summary>
        IEngineRepository<TEntity> Repository { get; }
        
        /// <summary>
        /// Perform asynchronous queries on an <see cref="IQueryable{T}"/> implementation.
        /// </summary>
        IAsyncQueryer AsyncQueryer { get; }
    }
}