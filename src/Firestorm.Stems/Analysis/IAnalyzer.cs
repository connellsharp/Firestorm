namespace Firestorm.Stems
{
    public interface IAnalyzer<in TDestination, in TSource>
        where TDestination : class
    {
        void Analyze(TDestination destination, TSource source);
    }
}