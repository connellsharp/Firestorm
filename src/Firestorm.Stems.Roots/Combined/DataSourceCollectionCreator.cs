using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Repositories;
using Firestorm.Engine.Subs.Wrapping;
using Firestorm.Stems.Fuel;

namespace Firestorm.Stems.Roots.Combined
{
    internal class DataSourceCollectionCreator<TItem> : IDataSourceCollectionCreator
        where TItem : class, new()
    {
        private readonly IDataSource _dataSource;

        public DataSourceCollectionCreator(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        IRestCollection IDataSourceCollectionCreator.GetRestCollection(Stem stem)
        {
            return GetRestCollection((Stem<TItem>) stem);
        }

        public IRestCollection GetRestCollection(Stem<TItem> stem)
        {
            IDataTransaction transaction = _dataSource.CreateTransaction();
            stem.OnDispose += delegate { transaction.Dispose(); };

            IEngineRepository<TItem> repository = _dataSource.GetRepository<TItem>(transaction);

            var wrapper = new DataEventWrapper<TItem>(transaction, repository);
            wrapper.TryWrapEvents(new StemDataChangeEvents<TItem>(stem));

            var subContext = new StemsEngineSubContext<TItem>(stem);
            var context = new FullEngineContext<TItem>(wrapper.Transaction, wrapper.Repository, subContext);

            return new EngineRestCollection<TItem>(context);
        }
    }
}