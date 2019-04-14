using Firestorm.Data;
using Firestorm.Engine;
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
            IDataContext<TItem> dataContext = _dataSource.CreateContext<TItem>();
            stem.OnDispose += delegate { dataContext.Transaction.Dispose(); };

            var subContext = new StemsEngineSubContext<TItem>(stem);
            var context = subContext.CreateFullContext(dataContext);

            return new EngineRestCollection<TItem>(context);
        }
    }
}