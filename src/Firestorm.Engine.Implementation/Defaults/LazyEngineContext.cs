using System;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;

namespace Firestorm.Engine
{
    public abstract class LazyEngineContext<TItem> : IEngineContext<TItem>
        where TItem : class
    {
        public IDataTransaction Transaction { get; }

        public IEngineRepository<TItem> Repository
        {
            get { throw new NotImplementedException(); }
        }

        public IIdentifierProvider<TItem> Identifiers
        {
            get { throw new NotImplementedException(); }
        }

        public IFieldProvider<TItem> Fields
        {
            get { throw new NotImplementedException(); }
        }

        public IAuthorizationChecker<TItem> AuthorizationChecker
        {
            get { throw new NotImplementedException(); }
        }

        public bool AllowsUpsert
        {
            get { throw new NotImplementedException(); }
        }
    }
}