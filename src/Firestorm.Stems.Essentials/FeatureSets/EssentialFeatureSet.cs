using System.Collections.Generic;
using System.Linq;
using Firestorm.Stems.Essentials.Factories.Resolvers;
using Firestorm.Stems.Fuel.Resolving.Analysis;

namespace Firestorm.Stems.Essentials
{
    /// <summary>
    /// Essential features for Firestorm Stems such as Expressions and runtime method resolvers.
    /// </summary>
    public class EssentialFeatureSet : IStemsFeatureSet
    {
        private IEnumerable<IFieldDefinitionResolver> Resolvers { get; } = new List<IFieldDefinitionResolver>
        {
            new ExpressionOnlyDefinitionResolver(),
            new RuntimeMethodDefinitionResolver(),
            new AuthorizePredicateResolver(),
            new DescriptionResolver(),
            new DisplayForResolver()
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