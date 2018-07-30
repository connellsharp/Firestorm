using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Defaults;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Repositories;

namespace Firestorm.Engine.Subs.Handlers
{
    public class SubItemFieldWriter<TItem, TNav> : IFieldWriter<TItem>
        where TItem : class
        where TNav : class, new()
    {
        private readonly SubWriterTools<TItem, TNav, TNav> _navTools;
        private readonly IEngineSubContext<TNav> _subContext;

        public SubItemFieldWriter(SubWriterTools<TItem, TNav, TNav> navTools, IEngineSubContext<TNav> subContext)
        {
            _navTools = navTools;
            _subContext = subContext;
        }

        public async Task SetValueAsync(IDeferredItem<TItem> item, object deserializedValue, IDataTransaction dataTransaction)
        {
            IEngineRepository<TNav> navRepository = new NavigationItemRepository<TItem, TNav>(item, _navTools.NavExpression, _navTools.Setter);

            var itemData = new RestItemData(deserializedValue);

            var navLocatorCreator = new NavigationItemLocatorCreator<TNav>(_subContext);
            DeferredItemBase<TNav> deferredItem = await navLocatorCreator.LocateOrCreateItemAsync(navRepository, itemData, item.LoadAsync);

            try
            {
                IDataTransaction transaction = new VoidTransaction(); // we commit the transaction in the parent. TODO optional save-as-you-go ?
                var navContext = new FullEngineContext<TNav>(transaction, navRepository, _subContext);
                var navEngineItem = new EngineRestItem<TNav>(navContext, deferredItem);
                Acknowledgment acknowledgment = await navEngineItem.EditAsync(itemData);
            }
            catch (RestApiException ex)
            {
                throw new SubWriterException(ex, item); 
            }
        }

        private class SubWriterException : RestApiException
        {
            public SubWriterException(RestApiException innerException, IDeferredItemInfo itemInfo)
                : base(innerException.ErrorStatus, GetMessage(itemInfo), innerException)
            { }

            private static string GetMessage(IDeferredItemInfo itemInfo)
            {
                string identifier = itemInfo.WasCreated ? "newly created "
                    : itemInfo.Identifier != null ? "'" + itemInfo.Identifier + "' "
                    : "";

                return "An error occured when editing the " + identifier + "sub item.";
            }
        }
    }
}