using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Engine;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;

namespace Firestorm.Fluent.Fuel
{
    internal class FluentEngineContext<TItem> : IEngineContext<TItem>
        where TItem : class
    {
        public FluentEngineContext(FieldImplementationsDictionary<TItem> implementations)
        {
            Fields = new FluentFieldProvider<TItem>(implementations);
        }

        public IDataTransaction Transaction { get; }
        public IEngineRepository<TItem> Repository { get; }
        public IIdentifierProvider<TItem> Identifiers { get; }
        public IFieldProvider<TItem> Fields { get; }
        public IAuthorizationChecker<TItem> AuthorizationChecker { get; }
    }
}
