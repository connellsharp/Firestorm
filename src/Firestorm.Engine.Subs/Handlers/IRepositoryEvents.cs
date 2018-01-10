namespace Firestorm.Engine.Subs.Handlers
{
    public interface IRepositoryEvents<TItem>
    {
        bool HasAnyEvent { get; }

        void OnCreating(TItem item);
    }
}