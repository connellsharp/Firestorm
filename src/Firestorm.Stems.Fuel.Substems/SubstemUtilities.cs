using Firestorm.Stems.Fuel.Resolving;

namespace Firestorm.Stems.Fuel.Substems
{
    internal static class SubstemUtilities
    {
        internal static StemsEngineSubContext<TNav> StemEngineContextFields<TItem, TNav, TSubstem>(Stem<TItem> stem)
            where TItem : class
            where TNav : class
            where TSubstem : Stem<TNav>
        {
            var autoActivator = new AutoActivator(stem.Configuration.DependencyResolver);
            var substem = autoActivator.CreateInstance<TSubstem>();
            substem.SetParent(stem);

            return new StemsEngineSubContext<TNav>(substem);
        }
    }
}