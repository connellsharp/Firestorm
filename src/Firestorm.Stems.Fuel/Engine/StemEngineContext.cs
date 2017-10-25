using Firestorm.Engine;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel
{
    public class StemEngineContext<TItem> : IEngineContext<TItem>
        where TItem : class
    {
        private readonly StemEngineSubContext<TItem> _stemEngineSubContext;

        public StemEngineContext(IDataTransaction transaction, IEngineRepository<TItem> repository, [NotNull] StemEngineSubContext<TItem> stemEngineSubContext)
        {
            Transaction = transaction;
            Repository = repository;

            _stemEngineSubContext = stemEngineSubContext;
        }

        public IDataTransaction Transaction { get; }

        public IEngineRepository<TItem> Repository { get; }

        public IIdentifierProvider<TItem> Identifiers => _stemEngineSubContext.IdentifierProvider;

        public IFieldProvider<TItem> Fields => _stemEngineSubContext.FieldProvider;

        public IAuthorizationChecker<TItem> AuthorizationChecker => _stemEngineSubContext.AuthorizationChecker;
    }
}