using Firestorm.Engine;
using Firestorm.Engine.Identifiers;
using Firestorm.Stems.Fuel.Authorization;
using Firestorm.Stems.Fuel.Fields;
using Firestorm.Stems.Fuel.Identifiers;
using Firestorm.Stems.Naming;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel
{
    /// <summary>
    /// Wraps a <see cref="Stem{TItem}"/> and provides API field information for the Firestorm Engine.
    /// Basically a <see cref="StemEngineContext{TItem}"/> without repository info.
    /// </summary>
    public class StemEngineSubContext<TItem>
        where TItem : class
    {
        private readonly Stem<TItem> _stem;

        public StemEngineSubContext([NotNull] Stem<TItem> stem)
        {
            _stem = stem;

            IdentifierProvider = new AttributeIdentifierProvider<TItem>(stem); //new IDConventionIdentifierInfo<TItem>();
            FieldProvider = new AttributeFieldProvider<TItem>(stem);
            AuthorizationChecker = new StemAuthorizationChecker<TItem>(stem);

            NamingConventionSwitcher conventionSwitcher = stem.Configuration.NamingConventionSwitcher;
            if (conventionSwitcher != null)
            {
                FieldProvider = new RelaxedNamingFieldProvider<TItem>(FieldProvider, conventionSwitcher);
                AuthorizationChecker = new RelaxedNamingAuthorizationChecker<TItem>(AuthorizationChecker, conventionSwitcher);
            }
        }

        public IIdentifierProvider<TItem> IdentifierProvider { get; }

        public IStemFieldProvider<TItem> FieldProvider { get; }

        public IAuthorizationChecker<TItem> AuthorizationChecker { get; }
    }
}