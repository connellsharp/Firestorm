using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Queryable;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Repositories;

namespace Firestorm.Engine.Subs.Handlers
{
    internal class NavigationItemLocatorCreator<TNav>
        where TNav : class, new()
    {
        private readonly IEngineSubContext<TNav> _subContext;

        internal NavigationItemLocatorCreator(IEngineSubContext<TNav> subContext)
        {
            _subContext = subContext;
        }

        /// <summary>
        /// Attempts to locate the item to modify using the modification request data.
        /// </summary>
        internal async Task<DeferredItemBase<TNav>> LocateOrCreateItemAsync(IEngineRepository<TNav> navRepository,
            RestItemData itemData, Func<Task> loadParentAsync, IAsyncQueryer queryer)
        {
            var locatedItem = LocateItem(navRepository, itemData);
            if (locatedItem != null)
                return locatedItem;

            await loadParentAsync();
            return new CreatableItem<TNav>(navRepository, queryer);
        }

        /// <summary>
        /// Attempts to locate the item to modify using the modification request data.
        /// </summary>
        internal DeferredItemBase<TNav> LocateItem(IEngineRepository<TNav> navRepository, RestItemData itemData)
        {
            foreach (string fieldName in itemData.Keys)
            {
                // TODO implicit & explicit location option e.g. { "*id": 123 }

                IItemLocator<TNav> locator = _subContext.Fields.GetLocator(fieldName);
                if (locator != null)
                {
                    object findValue = itemData[fieldName];

                    TNav locatedItem = locator.TryLocateItem(navRepository, findValue);
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
            var filter = new QueryableFieldFilter<TNav>(_subContext.Fields, filterInstructions);
            var itemQuery = filter.ApplyFilter(query).SingleDefferred();
            return new QueryableSingleRepository<TNav>(itemQuery);
        }

        private IEnumerable<FilterInstruction> GetFilterInstructions(RestItemData itemData)
        {
            foreach (string fieldName in itemData.Keys)
            {
                IItemLocator<TNav> locator = _subContext.Fields.GetLocator(fieldName);
                if (locator == null)
                    continue;

                string findValue = itemData[fieldName].ToString(); // TODO currently converts from int only to be parsed back later
                yield return new FilterInstruction(fieldName, FilterComparisonOperator.Equals, findValue);
            }
        }
    }
}