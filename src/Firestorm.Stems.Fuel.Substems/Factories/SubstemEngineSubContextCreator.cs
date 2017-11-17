using Firestorm.Stems.Fuel.Resolving;

namespace Firestorm.Stems.Fuel.Substems.Factories
{
    internal static class SubstemEngineSubContextCreator<TItem, TNav, TSubstem>
        where TItem : class 
        where TNav : class
        where TSubstem : Stem<TNav>
    {
        internal static StemEngineSubContext<TNav> StemEngineContextFields(Stem<TItem> stem)
        {
            var autoActivator = new AutoActivator(stem.Configuration.DependencyResolver);
            var substem = autoActivator.CreateInstance<TSubstem>();
            substem.SetParent(stem);

            return new StemEngineSubContext<TNav>(substem);
        }
    }
}