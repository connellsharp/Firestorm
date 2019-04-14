using Firestorm.Data;
using Firestorm.Engine.Identifiers;

namespace Firestorm.Engine.Subs.Context
{
    public interface IEngineSubContext<TItem>
        where TItem : class
    {
        IIdentifierProvider<TItem> Identifiers { get; }

        ILocatableFieldProvider<TItem> Fields { get; }

        IAuthorizationChecker<TItem> AuthorizationChecker { get; }

        IEngineContext<TItem> CreateFullContext(IDataContext<TItem> dataContext);
    }
}