using Firestorm.Engine.Identifiers;

namespace Firestorm.Engine.Subs.Context
{
    public interface IEngineSubContext<TItem>
        where TItem : class
    {
        IIdentifierProvider<TItem> IdentifierProvider { get; }
        ILocatableFieldProvider<TItem> FieldProvider { get; }
        IAuthorizationChecker<TItem> AuthorizationChecker { get; }
    }
}