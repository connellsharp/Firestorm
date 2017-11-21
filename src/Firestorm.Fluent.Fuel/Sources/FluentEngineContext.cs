using System.Collections.Generic;
using Firestorm.Engine;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using Firestorm.Fluent.Fuel.Definitions;

namespace Firestorm.Fluent.Fuel.Sources
{
    internal class FluentEngineContext<TItem> : IEngineContext<TItem>
        where TItem : class
    {
        public FluentEngineContext(IDictionary<string, ApiFieldModel<TItem>> fieldModels)
        {
            Fields = new FluentFieldProvider<TItem>(fieldModels);
        }

        public IDataTransaction Transaction { get; }
        public IEngineRepository<TItem> Repository { get; }
        public IIdentifierProvider<TItem> Identifiers { get; }
        public IFieldProvider<TItem> Fields { get; }
        public IAuthorizationChecker<TItem> AuthorizationChecker { get; }
    }
}
