using Firestorm.Data;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using JetBrains.Annotations;

namespace Firestorm.Engine.Subs.Context
{
    public class FullEngineContext<TItem> : IEngineContext<TItem>
        where TItem : class
    {
        private readonly IDataContext<TItem> _dataContext;
        private readonly IEngineSubContext<TItem> _subContext;

        public FullEngineContext([NotNull] IDataContext<TItem> dataContext, [NotNull] IEngineSubContext<TItem> subContext)
        {
            _dataContext = dataContext;
            _subContext = subContext;
        }

        public IDataContext<TItem> Data => _dataContext;

        public IIdentifierProvider<TItem> Identifiers => _subContext.Identifiers;

        public IFieldProvider<TItem> Fields => _subContext.Fields;

        public IAuthorizationChecker<TItem> AuthorizationChecker => _subContext.AuthorizationChecker;
    }
}