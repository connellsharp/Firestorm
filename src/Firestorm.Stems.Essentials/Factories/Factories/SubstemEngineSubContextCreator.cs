using Firestorm.Stems.Fuel;
using Firestorm.Stems.Fuel.Resolving;

namespace Firestorm.Stems.Essentials.Factories.Factories
{
    internal class SubstemEngineSubContextCreator<TItem, TNav, TSubstem>
        where TItem : class 
        where TNav : class
        where TSubstem : Stem<TNav>
    {
        private readonly Stem<TItem> _stem;
        private readonly TSubstem _substem;

        public SubstemEngineSubContextCreator(Stem<TItem> stem)
        {
            _stem = stem;

            var autoActivator = new AutoActivator(_stem.Configuration.DependencyResolver);
            _substem = autoActivator.CreateInstance<TSubstem>();
            _substem.SetParent(_stem);
        }

        internal StemsEngineSubContext<TNav> GetEngineContext()
        {
            return new StemsEngineSubContext<TNav>(_substem);
        }

        internal StemDataChangeEvents<TNav> GetDataChangeEvents()
        {
            return new StemDataChangeEvents<TNav>( _substem);
        }
    }
}