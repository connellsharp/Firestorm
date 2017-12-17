using System.Collections.Generic;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Authorization;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using Firestorm.Fluent.Fuel.Models;

namespace Firestorm.Fluent.Fuel.Engine
{
    internal class FluentEngineContext<TItem> : IEngineContext<TItem>
        where TItem : class, new()
    {
        public FluentEngineContext(IDataSource dataSource, IDictionary<string, ApiFieldModel<TItem>> fieldModels, IDictionary<string, ApiIdentifierModel<TItem>> identifierModels)
        {
            Transaction = dataSource.CreateTransaction();
            Repository = dataSource.GetRepository<TItem>(Transaction);

            Identifiers = new FluentIdentifierProvider<TItem>(identifierModels);
            Fields = new FluentFieldProvider<TItem>(fieldModels);
            
            AuthorizationChecker = new AllowAllAuthorizationChecker<TItem>(); // TODO
        }

        public IDataTransaction Transaction { get; }

        public IEngineRepository<TItem> Repository { get; }

        public IIdentifierProvider<TItem> Identifiers { get; }

        public IFieldProvider<TItem> Fields { get; }

        public IAuthorizationChecker<TItem> AuthorizationChecker { get; }
    }
}
