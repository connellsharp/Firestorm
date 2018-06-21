using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Engine.Fields;

namespace Firestorm.Engine.Additives.Readers
{
    internal class DelegateWholeItemReplacer<TItem, TValue> : IFieldValueReplacer<TItem>
        where TItem : class
    {
        private readonly Func<TItem, TValue> _getterFunc;

        public DelegateWholeItemReplacer(Func<TItem, TValue> getterFunc)
        {
            _getterFunc = getterFunc ?? throw new ArgumentNullException(nameof(getterFunc));
        }

        public Task LoadAsync(IQueryable<TItem> itemsQuery)
        {
            return Task.FromResult(false);
        }

        public object GetReplacement(object dbValue)
        {
            return _getterFunc((TItem) dbValue);
        }
    }
}