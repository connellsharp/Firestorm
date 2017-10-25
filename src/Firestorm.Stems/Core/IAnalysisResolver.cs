namespace Firestorm.Stems
{
    /// <summary>
    /// The base interface for analysis resolvers.
    /// </summary>
    /// <remarks>
    /// Usually casted into derived interfaces. Not great design, I know. Didn't want all the FieldDefinition stuff in Firestorm.Stems.
    /// </remarks>
    public interface IAnalysisResolver
    {
        IStemConfiguration Configuration { set; }
    }
}