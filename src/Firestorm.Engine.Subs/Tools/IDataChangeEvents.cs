using System.Threading.Tasks;

namespace Firestorm.Engine.Subs
{
    /// <summary>
    /// Events that are called when data is changed e.g. updated, deleted, created, etc.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public interface IDataChangeEvents<in TItem>
    {
        bool HasAnyEvent { get; }

        void OnCreating(TItem newItem);

        void OnUpdating(TItem item);

        void OnDeleting(TItem item);

        Task OnSavingAsync(TItem item);

        Task OnSavedAsync(TItem item);
    }
}