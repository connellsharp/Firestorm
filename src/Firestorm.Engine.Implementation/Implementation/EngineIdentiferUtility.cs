using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;

namespace Firestorm.Engine
{
    internal static class EngineIdentiferUtility
    {
        internal static IDeferredItem<TItem> GetIdentifiedItem<TItem>(IEngineContext<TItem> engineContext, IIdentifierInfo<TItem> identifierInfo, string identifier)
            where TItem : class
        {
            TItem alreadyLoadedItem = identifierInfo.GetAlreadyLoadedItem(identifier);
            if (alreadyLoadedItem != null)
                return new AlreadyLoadedItem<TItem>(alreadyLoadedItem, identifier);

            return new IdentifiedItem<TItem>(identifier, engineContext.Repository, identifierInfo);
        }
    }
}