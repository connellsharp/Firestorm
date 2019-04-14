using System;
using Firestorm.Data;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using JetBrains.Annotations;

namespace Firestorm.Engine.Defaults
{
    /// <summary>
    /// <see cref="IEngineContext{TItem}"/> where all dependencies are passed into the constructor.
    /// </summary>
    public class InjectedEngineContext<TItem> : IEngineContext<TItem>
        where TItem : class
    {
        public InjectedEngineContext([NotNull] IDataTransaction transaction, [NotNull] IEngineRepository<TItem> repository,
            [NotNull] IAsyncQueryer asyncQueryer,
            [NotNull] IIdentifierProvider<TItem> identifiers, [NotNull] IFieldProvider<TItem> fields,
            [NotNull] IAuthorizationChecker<TItem> authorizationChecker)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (fields == null) throw new ArgumentNullException(nameof(fields));
            if (authorizationChecker == null) throw new ArgumentNullException(nameof(authorizationChecker));

            Data = new DataContext<TItem>
            {
                Transaction = transaction,
                Repository = repository,
                AsyncQueryer = asyncQueryer
            };
            
            Identifiers = identifiers;
            Fields = fields;
            AuthorizationChecker = authorizationChecker;
        }

        public IDataContext<TItem> Data { get; }
        
        public IIdentifierProvider<TItem> Identifiers { get; }

        public IFieldProvider<TItem> Fields { get; }

        public IAuthorizationChecker<TItem> AuthorizationChecker { get; }

        public bool AllowsUpsert
        {
            // TODO: config?
            get { return true; }
        }
    }
}