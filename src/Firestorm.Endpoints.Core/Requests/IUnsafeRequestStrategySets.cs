namespace Firestorm.Endpoints.Requests
{
    /// <summary>
    /// Contains 3 sets of strategies (for collection, items and scalars) defining how endpoints behave to unsafe requests.
    /// </summary>
    public interface IUnsafeRequestStrategySets
    {
        UnsafeRequestStrategies<IRestCollection> ForCollections { get; set; }

        UnsafeRequestStrategies<IRestItem> ForItems { get; set; }

        UnsafeRequestStrategies<IRestScalar> ForScalars { get; set; }
    }
}