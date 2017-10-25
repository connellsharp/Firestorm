using System.Linq;
using System.Threading.Tasks;

namespace Firestorm.Engine
{
    /// <summary>
    /// An item that has already been created from a POST request.
    /// </summary>
    /// <remarks>
    /// Only really here to pass into the <see cref="IAuthorizationChecker{TItem}"/> methods.
    /// </remarks>
    public class PostedNewItem<TItem> : IDeferredItem<TItem>
    {
        public PostedNewItem(TItem newItem)
        {
            Query = new QueryableSingle<TItem>(new[] { newItem }.AsQueryable());
            LoadedItem = newItem;
        }

        public string Identifier { get; } = null;

        public IQueryableSingle<TItem> Query { get; }

        public TItem LoadedItem { get; }

        public Task LoadAsync()
        {
            return Task.FromResult(false);
        }

        public bool WasCreated { get; } = true;
    }
}