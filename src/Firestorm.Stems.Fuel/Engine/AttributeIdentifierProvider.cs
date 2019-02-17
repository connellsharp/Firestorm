using System.Linq;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Identifiers;
using Firestorm.Stems.Fuel.Resolving.Analysis;

namespace Firestorm.Stems.Fuel
{
    /// <summary>
    /// Builds <see cref="IIdentifierInfo{TItem}"/> from <see cref="IdentifierAttribute"/>s placed on properties and predicate methods in a Stem.
    /// </summary>
    internal class AttributeIdentifierProvider<TItem> : IIdentifierProvider<TItem>
        where TItem : class
    {
        private readonly Stem<TItem> _stem;
        private readonly EngineImplementations<TItem> _implementations;

        public AttributeIdentifierProvider(Stem<TItem> stem, EngineImplementations<TItem> implementations)
        {
            _stem = stem;
            _implementations = implementations;
        }

        public IIdentifierInfo<TItem> GetInfo(string identifierName)
        {
            if (identifierName == null)
            {
                if(_implementations.IdentifierFactories.Count == 0)
                    return new IdConventionIdentifierInfo<TItem>();

                var infos = _implementations.IdentifierFactories.Values.Select(i => i.Get(_stem));
                return new CombinedIdentifierInfo<TItem>(infos);
            }
            else
            {
                var info = _implementations.IdentifierFactories[identifierName].Get(_stem);
                return info;
            }
        }
    }
}