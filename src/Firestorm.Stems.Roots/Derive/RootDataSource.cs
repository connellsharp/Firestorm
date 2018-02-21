using Firestorm.Data;
using Firestorm.Stems.Roots.Combined;

namespace Firestorm.Stems.Roots.Derive
{
    public class RootDataSource : IDataSource
    {
        private readonly Root _root;

        public RootDataSource(Root root)
        {
            _root = root;
        }

        public IDataTransaction CreateTransaction()
        {
            return new RootDataTransaction(_root);
        }

        public IEngineRepository<TEntity> GetRepository<TEntity>(IDataTransaction transaction)
            where TEntity : class, new()
        {
            return new RootEngineRepository<TEntity>((Root<TEntity>)_root);
        }
    }
}