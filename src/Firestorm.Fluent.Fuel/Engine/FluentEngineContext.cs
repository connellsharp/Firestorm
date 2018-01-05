using System.Collections.Generic;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using Firestorm.Fluent.Fuel.Models;

namespace Firestorm.Fluent.Fuel.Engine
{
    internal class FluentEngineContext<TItem> : IEngineContext<TItem>
        where TItem : class, new()
    {
        private readonly FluentEngineSubContext<TItem> _subContext;

        public FluentEngineContext(IDataSource dataSource, IDictionary<string, ApiFieldModel<TItem>> fieldModels, IDictionary<string, ApiIdentifierModel<TItem>> identifierModels)
        {
            // TODO refactor this ctor ? Just use the FullEngineContext and pass in the FluentEngineSubContext ?

            Transaction = dataSource.CreateTransaction();
            Repository = dataSource.GetRepository<TItem>(Transaction);

            _subContext = new FluentEngineSubContext<TItem>(fieldModels, identifierModels);
        }

        public IDataTransaction Transaction { get; }

        public IEngineRepository<TItem> Repository { get; }

        public IIdentifierProvider<TItem> Identifiers => _subContext.Identifiers;

        public IFieldProvider<TItem> Fields => _subContext.Fields;

        public IAuthorizationChecker<TItem> AuthorizationChecker => _subContext.AuthorizationChecker;
    }
}
