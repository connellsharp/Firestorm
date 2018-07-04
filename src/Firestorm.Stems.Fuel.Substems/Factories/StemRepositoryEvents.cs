using Firestorm.Engine.Subs;

namespace Firestorm.Stems.Fuel.Substems.Factories
{
    internal class StemRepositoryEvents<TItem> : IRepositoryEvents<TItem>
        where TItem : class
    {
        private readonly Stem<TItem> _stem;

        public StemRepositoryEvents(Stem<TItem> stem)
        {
            _stem = stem;
        }

        public bool HasAnyEvent { get; }

        public void OnCreating(TItem item)
        {
            _stem.OnCreating(item);
        }
    }
}