namespace Firestorm.Stems
{
    /// <summary>
    /// Creates <see cref="IAnalyzer"/> instances for a supplied analyzer type.
    /// </summary>
    public interface IAnalyzerFactory
    {
        IAnalyzer<TDestination, TSource> GetAnalyzer<TDestination, TSource>()
            where TDestination : class;
    }
}