namespace Firestorm.Engine.Data
{
    /// <summary>
    /// Provides a simple source interface to create data transactions and get repositories used in the Engine.
    /// </summary>
    /// <remarks>
    /// There will usually be one instance of this per application.
    /// </remarks>
    public interface IDataSource
    {
        IDataTransaction CreateTransaction();

        IEngineRepository<TEntity> GetRepository<TEntity>(IDataTransaction transaction)
            where TEntity : class, new();
    }
}