using System;
using Firestorm.Engine;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;

namespace Firestorm.Fluent.Fuel
{
    internal class FluentEngineContext<TItem> : IEngineContext<TItem>
        where TItem : class
    {
        public IDataTransaction Transaction { get; }
        public IEngineRepository<TItem> Repository { get; }
        public IIdentifierProvider<TItem> Identifiers { get; }
        public IFieldProvider<TItem> Fields { get; }
        public IAuthorizationChecker<TItem> AuthorizationChecker { get; }
    }
}
