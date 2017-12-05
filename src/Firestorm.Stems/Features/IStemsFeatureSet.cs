using System.Collections.Generic;

namespace Firestorm.Stems
{
    /// <summary>
    /// A set of features to extend Firestorm Stems.
    /// </summary>
    public interface IStemsFeatureSet
    {
        IEnumerable<TResolver> GetResolvers<TResolver>()
            where TResolver : IAnalysisResolver;
    }
}