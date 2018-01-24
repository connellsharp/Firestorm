using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Fields;

namespace Firestorm.Engine.Subs
{
    public class NavigationSetterFieldWriter<TItem, TNav> : IFieldWriter<TItem>
        where TItem : class
    {
        private readonly INavigationSetter<TItem, TNav> _setter;

        public NavigationSetterFieldWriter(INavigationSetter<TItem, TNav> setter)
        {
            _setter = setter;
        }

        public async Task SetValueAsync(IDeferredItem<TItem> item, object deserializedValue, IDataTransaction dataTransaction)
        {
            TNav navValue = ConversionUtility.ConvertValue<TNav>(deserializedValue);

            await item.LoadAsync();

            _setter.SetNavItem(item.LoadedItem, navValue);
        }
    }
}