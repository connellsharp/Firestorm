using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Subs.Context;
using Firestorm.Stems.Fuel;

namespace Firestorm.Stems.Roots.DataSource
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

            var stemEngineInfo = new StemsEngineSubContext<TItem>(stem);
            var context = new AdditiveEngineContext<TItem>(transaction, repository, stemEngineInfo);

            return new EngineRestCollection<TItem>(context);
        }
    }
}