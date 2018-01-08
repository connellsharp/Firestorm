using System.Collections.Generic;
using System.Linq;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Authorization;
using Firestorm.Engine.Identifiers;
using Firestorm.Engine.Subs.Context;
using Firestorm.Fluent.Fuel.Models;

namespace Firestorm.Fluent.Fuel.Engine
{
    internal class FluentEngineSubContext<TItem> : IEngineSubContext<TItem>
        where TItem : class, new()
    {
        public FluentEngineSubContext(IEnumerable<ApiFieldModel<TItem>> fieldModels, IEnumerable<ApiIdentifierModel<TItem>> identifierModels)
        {
            Identifiers = new FluentIdentifierProvider<TItem>(identifierModels);
            Fields = new FluentFieldProvider<TItem>(fieldModels);
            
            AuthorizationChecker = new AllowAllAuthorizationChecker<TItem>(); // TODO
        }

        public IIdentifierProvider<TItem> Identifiers { get; }

        public ILocatableFieldProvider<TItem> Fields { get; }

        public IAuthorizationChecker<TItem> AuthorizationChecker { get; }
    }
}