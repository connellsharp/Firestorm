using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Queryable;
using Firestorm.Engine.Subs.Context;
using Reflectious;

namespace Firestorm.Engine.Subs.Handlers
{
    public class SubCollectionReplacer<TItem, TNav> : IFieldValueReplacer<TItem> 
        where TNav : class
    {
        private readonly Func<IQueryable<TItem>, IQueryable<IEnumerable<TNav>>> _selectAllNavFunc;
        private readonly FieldReplacementProcessor<TNav> _replacementProcessor;
        
        public SubCollectionReplacer(IEngineSubContext<TNav> engineSubContext, Func<IQueryable<TItem>, IQueryable<IEnumerable<TNav>>> selectAllNavFunc)
        {
            _selectAllNavFunc = selectAllNavFunc;
            _replacementProcessor = new FieldReplacementProcessor<TNav>(() => engineSubContext.Fields.GetReaders(1));
        }

        public Task LoadAsync(IQueryable<TItem> itemsQuery)
        {
            IQueryable<TNav> navQuery = _selectAllNavFunc(itemsQuery).SelectMany(n => n);
            return  _replacementProcessor.LoadAllAsync(navQuery);
        }

        public object GetReplacement(object dbValue)
        {
            var enumerableType = dbValue.GetType().GetGenericInterface(typeof(IEnumerable<>));
            var itemType = enumerableType.GetGenericArguments()[0];

            throw new NotImplementedException("Not completed implementation for sub collection replacers yet.");
            // Note: somehow the test for Stems didn't fail until the split for Items and Collection replacers

            var dbList = ((IEnumerable) dbValue).OfType<object>().ToList();

            foreach (object dbItem in dbList)
            {
                _replacementProcessor.Replace(dbItem, itemType);
            }

            return dbList;
        }
    }
}