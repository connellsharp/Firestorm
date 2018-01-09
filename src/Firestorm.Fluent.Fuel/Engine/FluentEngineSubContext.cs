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
        internal FluentEngineSubContext(ApiItemModel<TItem> itemModel)
        {
            Identifiers = new FluentIdentifierProvider<TItem>(itemModel.Identifiers);
            Fields = new FluentFieldProvider<TItem>(itemModel.Fields);
            
            AuthorizationChecker = new AllowAllAuthorizationChecker<TItem>(); // TODO
        }

        public IIdentifierProvider<TItem> Identifiers { get; }

        public ILocatableFieldProvider<TItem> Fields { get; }

        public IAuthorizationChecker<TItem> AuthorizationChecker { get; }
    }
}