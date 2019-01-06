using System.Threading.Tasks;
using Firestorm.Engine.Subs;

namespace Firestorm.Stems.Fuel
{
    public class StemDataChangeEvents<TItem> : IDataChangeEvents<TItem>
        where TItem : class
    {
        private readonly Stem<TItem> _stem;

        public StemDataChangeEvents(Stem<TItem> stem)
        {
            _stem = stem;
        }

        public bool HasAnyEvent { get; } = true;

        public void OnCreating(TItem newItem)
        {
            _stem.OnCreating(newItem);
        }

        public void OnUpdating(TItem item)
        {
            _stem.OnUpdating(item);
        }

        public DeletingResult OnDeleting(TItem item)
        {
            return _stem.MarkDeleted(item) ? DeletingResult.Handled : DeletingResult.Continue;
        }

        public Task OnSavingAsync(TItem item)
        {
            return _stem.OnSavingAsync(item);
        }

        public Task OnSavedAsync(TItem item)
        {
            return _stem.OnSavedAsync(item);
        }
    }
}