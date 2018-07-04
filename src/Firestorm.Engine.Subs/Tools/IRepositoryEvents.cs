namespace Firestorm.Engine.Subs
{
    public interface IRepositoryEvents<in TItem>
    {
        bool HasAnyEvent { get; }

        void OnCreating(TItem item);

        void OnDeleting(TItem item);
    }
}