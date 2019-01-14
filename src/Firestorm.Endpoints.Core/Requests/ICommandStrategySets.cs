namespace Firestorm.Endpoints.Requests
{
    /// <summary>
    /// Contains 3 sets of strategies (for collection, items and scalars) defining how endpoints behave to unsafe requests.
    /// </summary>
    public interface ICommandStrategySets
    {
        CommandStrategies<IRestCollection> ForCollections { get; set; }

        CommandStrategies<IRestItem> ForItems { get; set; }

        CommandStrategies<IRestScalar> ForScalars { get; set; }
    }
}