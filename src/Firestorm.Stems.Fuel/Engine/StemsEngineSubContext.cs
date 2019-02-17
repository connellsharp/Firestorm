using Firestorm.Engine;
using Firestorm.Engine.Identifiers;
using Firestorm.Engine.Subs.Context;
using Firestorm.Stems.Fuel.Resolving.Analysis;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel
{
    /// <summary>
    /// Wraps a <see cref="Stem{TItem}"/> and provides API field information for the Firestorm Engine.
    /// Basically a <see cref="FullEngineContext{TItem}"/> without repository info.
    /// </summary>
    public class StemsEngineSubContext<TItem> : IEngineSubContext<TItem>
        where TItem : class
    {
        public StemsEngineSubContext([NotNull] Stem<TItem> stem)
        {
            var implementations = stem.Services.ServiceGroup
                .GetProvider(stem.GetType())
                .GetService<EngineImplementations<TItem>>();

            Identifiers = new AttributeIdentifierProvider<TItem>(stem, implementations);
            Fields = new AttributeFieldProvider<TItem>(stem, implementations);
            AuthorizationChecker = new StemAuthorizationChecker<TItem>(stem, implementations);
        }

        public IIdentifierProvider<TItem> Identifiers { get; }

        public ILocatableFieldProvider<TItem> Fields { get; }

        public IAuthorizationChecker<TItem> AuthorizationChecker { get; }
    }
}