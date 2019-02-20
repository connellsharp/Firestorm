using Firestorm.Stems.Fuel.Analysis;

namespace Firestorm.Stems.Analysis
{
    /// <summary>
    /// Interface to ensure analyzer classes follow a common pattern.
    /// </summary>
    /// <remarks>
    /// This is not the same as the <see cref="IDefinitionAnalyzer{TDefinition}"/>, but is a similar pattern.
    /// </remarks>
    internal interface IAnalyzer<in TDestination, in TSource>
        where TDestination : class
    {
        void Analyze(TDestination destination, TSource source);
    }
}