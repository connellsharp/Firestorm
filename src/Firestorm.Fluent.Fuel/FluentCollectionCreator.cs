using System.Collections.Generic;
using Firestorm.Engine;
using Firestorm.Engine.Data;

namespace Firestorm.Fluent.Fuel
{
    internal class FluentCollectionCreator<TItem> : IFluentCollectionCreator
        where TItem : class
    {
        private readonly IDataSource _dataSource;

        private readonly FieldImplementationsDictionary<TItem> _fieldImplementations;

        internal FluentCollectionCreator(IDataSource dataSource, FieldImplementationsDictionary<TItem> fieldImplementations)
        {
            _dataSource = dataSource;
            _fieldImplementations = fieldImplementations;
        }

        public IRestCollection GetRestCollection()
        {
            IEngineContext<TItem> context = new FluentEngineContext<TItem>(_fieldImplementations);
            return new EngineRestCollection<TItem>(context);
        }
    }
}