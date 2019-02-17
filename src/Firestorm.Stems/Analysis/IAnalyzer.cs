namespace Firestorm.Stems.Analysis
{
    /// <summary>
    /// Interface to ensure analyzer classes follow a common pattern.
    /// </summary>
    internal interface IAnalyzer<in TDestination, in TSource>
        where TDestination : class
    {
        void Analyze(TDestination destination, TSource source);
    }
}