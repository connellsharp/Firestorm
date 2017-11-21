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

            //foreach (var field in apiItemBuilder.Fields) {
            //    implementations.Add(field.Name, new FieldImplementations<TItem> {
            //        Reader = new ExpressionFieldReader<TItem>(field.Expression),
            //        Writer = new ExpressionFieldWriter<TItem>(field.Expression),
            //        FullResource = new ExpressionFullResource(field.Expression, )
            //    });
            //}

            return new FluentCollectionCreator<TItem>(_dataSource, implementations);
        }
    }
}