using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Queryable;
using Firestorm.Engine.Subs.Context;

namespace Firestorm.Engine.Subs.Handlers
{
    public class SubFieldReplacer<TItem, TNav> : IFieldValueReplacer<TItem> 
        where TNav : class
    {
        private readonly Func<IQueryable<TItem>, IQueryable<TNav>> _selectAllNavFunc;
        private readonly FieldReplacementProcessor<TNav> _replacementProcessor;
        
        public SubFieldReplacer(IEngineSubContext<TNav> engineSubContext, Func<IQueryable<TItem>, IQueryable<TNav>> selectAllNavFunc)
        {
            _selectAllNavFunc = selectAllNavFunc;
            _replacementProcessor = new FieldReplacementProcessor<TNav>(() => engineSubContext.Fields.GetReaders(1));
        }

        public Task LoadAsync(IQueryable<TItem> itemsQuery)
        {
            IQueryable<TNav> navQuery = _selectAllNavFunc(itemsQuery);
            return  _replacementProcessor.LoadAllAsync(navQuery);
        }

        public object GetReplacement(object dbValue)
        {
            _replacementProcessor.Replace(dbValue, dbValue.GetType());
            return dbValue;
        }
    }
}