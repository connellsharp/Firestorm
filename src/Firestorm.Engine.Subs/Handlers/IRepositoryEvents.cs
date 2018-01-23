namespace Firestorm.Engine.Subs.Handlers
{
    public interface IRepositoryEvents<in TItem>
    {
        bool HasAnyEvent { get; }

        void OnCreating(TItem item);
    }
}