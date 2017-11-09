using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Engine;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Queryable;
using Firestorm.Stems.Fuel.Fields;
using Firestorm.Stems.Fuel.Substems.Repositories;

namespace Firestorm.Stems.Fuel.Substems.Handlers
{
    internal class StemNavigationItemLocatorCreator<TNav>
        where TNav : class, new()
    {
        private readonly StemEngineSubContext<TNav> _substemSubContext;

        internal StemNavigationItemLocatorCreator(StemEngineSubContext<TNav> substemSubContext)
        {
            _substemSubContext = substemSubContext;
        }

        /// <summary>
        /// Attempts to locate the item to modify using the modification request data.
        /// </summary>
        internal async Task<DeferredItemBase<TNav>> LocateOrCreateItemAsync(StemEngineContext<TNav> navContext, RestItemData itemData, Func<Task> loadParentAsync)
        {
            var locatedItem = LocateItem(navContext, itemData);
            if (locatedItem != null)
                return locatedItem;

            await loadParentAsync();
            return new CreatableItem<TNav>(navContext.Repository);
        }

        /// <summary>
        /// Attempts to locate the item to modify using the modification request data.
        /// </summary>
        internal DeferredItemBase<TNav> LocateItem(StemEngineContext<TNav> navContext, RestItemData itemData)
        {
            foreach (string fieldName in itemData.Keys)
            {
                // TODO implicit & explicit location option e.g. { "*id": 123 }

                IItemLocator<TNav> locator = _substemSubContext.FieldProvider.GetLocator(fieldName);
                if (locator != null)
                {
                    object findValue = itemData[fieldName];

                    TNav locatedItem = locator.TryLocateItem(navContext.Repository, findValue);
                    if (locatedItem != null)
                    {
                        itemData.Remove(fieldName); // remove so we don't write to this value later.
                        return new LoadedItem<TNav>(locatedItem);
                    }
                }
            }

            return null;
        }

        private QueryableSingleRepository<TNav> LocateItemByFilters(IQueryable<TNav> query, RestItemData itemData)
        {
            IEnumerable<FilterInstruction> filterInstructions = GetFilterInstructions(itemData);
            var filter = new QueryableFieldFilter<TNav>(_substemSubContext.FieldProvider, filterInstructions);
            var itemQuery = filter.ApplyFilter(query).SingleDefferred();
            return new QueryableSingleRepository<TNav>(itemQuery);
        }

        private IEnumerable<FilterInstruction> GetFilterInstructions(RestItemData itemData)
        {
            foreach (string fieldName in itemData.Keys)
            {
                IItemLocator<TNav> locator = _substemSubContext.FieldProvider.GetLocator(fieldName);
                if (locator == null)
                    continue;

                string findValue = itemData[fieldName].ToString(); // todo currently converts from int only to be parsed back later
                yield return new FilterInstruction(fieldName, FilterComparisonOperator.Equals, findValue);
            }
        }
    }
}