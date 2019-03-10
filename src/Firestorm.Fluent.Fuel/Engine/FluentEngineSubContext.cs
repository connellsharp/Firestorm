using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Authorization;
using Firestorm.Engine.Identifiers;
using Firestorm.Engine.Subs;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Wrapping;
using Firestorm.Fluent.Fuel.Models;

namespace Firestorm.Fluent.Fuel.Engine
{
    internal class FluentEngineSubContext<TItem> : IEngineSubContext<TItem>
        where TItem : class, new()
    {
        private readonly IDataChangeEvents<TItem> _events;

        internal FluentEngineSubContext(ApiItemModel<TItem> itemModel)
        {
            Identifiers = new FluentIdentifierProvider<TItem>(itemModel.Identifiers);
            Fields = new FluentFieldProvider<TItem>(itemModel.Fields);
            
            AuthorizationChecker = new AllowAllAuthorizationChecker<TItem>(); // TODO

            _events = itemModel.Events;
        }

        public IIdentifierProvider<TItem> Identifiers { get; }

        public ILocatableFieldProvider<TItem> Fields { get; }

        public IAuthorizationChecker<TItem> AuthorizationChecker { get; }
        
        public IEngineContext<TItem> CreateFullContext(IDataTransaction transaction, IEngineRepository<TItem> repository)
        {
            var eventWrapper = new DataEventWrapper<TItem>(transaction, repository);
            eventWrapper.TryWrapEvents(_events);

            return new FullEngineContext<TItem>(eventWrapper.Transaction, eventWrapper.Repository, this);
        }
    }
}