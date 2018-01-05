using Firestorm.Data;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using JetBrains.Annotations;

namespace Firestorm.Engine.Subs.Context
{
    public class FullEngineContext<TItem> : IEngineContext<TItem>
        where TItem : class
    {
        private readonly IEngineSubContext<TItem> _subContext;

        public FullEngineContext(IDataTransaction transaction, IEngineRepository<TItem> repository, [NotNull] IEngineSubContext<TItem> subContext)
        {
            Transaction = transaction;
            Repository = repository;

            _subContext = subContext;
        }

        public IDataTransaction Transaction { get; }

        public IEngineRepository<TItem> Repository { get; }

        public IIdentifierProvider<TItem> Identifiers => _subContext.Identifiers;

        public IFieldProvider<TItem> Fields => _subContext.Fields;

        public IAuthorizationChecker<TItem> AuthorizationChecker => _subContext.AuthorizationChecker;
    }
}