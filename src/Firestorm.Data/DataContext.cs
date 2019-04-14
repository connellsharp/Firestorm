namespace Firestorm.Data
{
    public class DataContext<TEntity> : IDataContext<TEntity> 
        where TEntity : class
    {
        public IDataTransaction Transaction { get; set; }
        
        public IEngineRepository<TEntity> Repository { get; set; }
        
        public IAsyncQueryer AsyncQueryer { get; set; }
    }
}