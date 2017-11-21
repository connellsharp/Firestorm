using Firestorm.Engine.Data;

namespace Firestorm.Fluent.Fuel
{
    public class FluentCollectionCreatorCreator : IFluentCollectionCreatorCreator
    {
        private readonly IDataSource _dataSource;

        public FluentCollectionCreatorCreator(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public IFluentCollectionCreator GetCollectionCreator<TItem>(ApiItemBuilder<TItem> apiItemBuilder)
            where TItem : class
        {
            var implementations = new FieldImplementationsDictionary<TItem>();

            // TODO builder implementations from builder

            return new FluentCollectionCreator<TItem>(_dataSource, implementations);
        }
    }
}