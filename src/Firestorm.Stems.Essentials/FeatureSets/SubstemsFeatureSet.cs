using System.Collections.Generic;
using System.Linq;
using Firestorm.Stems.Essentials.Factories.Resolvers;
using Firestorm.Stems.Fuel.Resolving.Analysis;

namespace Firestorm.Stems.Essentials
{
    /// <summary>
    /// Feature set for substems.
    /// </summary>
    public class SubstemsFeatureSet : IStemsFeatureSet
    {
        private IEnumerable<IFieldDefinitionResolver> Resolvers { get; } = new List<IFieldDefinitionResolver>
        {
            new SubstemDefinitionResolver(),
        };

        public IEnumerable<TResolver> GetResolvers<TResolver>()
            where TResolver : IAnalysisResolver
        {
            if (typeof(TResolver) == typeof(IFieldDefinitionResolver))
                return Resolvers.OfType<TResolver>();

            return null;
        }
    }
}