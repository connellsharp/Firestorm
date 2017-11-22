using Firestorm.Engine;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using Firestorm.Stems.Fuel.Fields;

namespace Firestorm.Stems.Fuel
{
    public interface IEngineSubContext<TItem>
        where TItem : class
    {
        IIdentifierProvider<TItem> IdentifierProvider { get; }
        ILocatableFieldProvider<TItem> FieldProvider { get; }
        IAuthorizationChecker<TItem> AuthorizationChecker { get; }
    }
}