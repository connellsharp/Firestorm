using Firestorm.Endpoints.Requests;

namespace Firestorm.Endpoints.Strategies
{
    /// <summary>
    /// Contains 3 sets of strategies (for collection, items and scalars) defining how endpoints behave to unsafe requests.
    /// </summary>
    public class CommandStrategySets : ICommandStrategySets
    {
        public CommandStrategies<IRestCollection> ForCollections { get; set; } = new CommandStrategies<IRestCollection>
        {
            { UnsafeMethod.Post, new AddToCollectionStrategy() },
        };

        public CommandStrategies<IRestItem> ForItems { get; set; } = new CommandStrategies<IRestItem>
        {
            { UnsafeMethod.Put, new PartialUpdateItemStrategy() },
            { UnsafeMethod.Patch, new PartialUpdateItemStrategy() },
            { UnsafeMethod.Delete, new DeleteItemStrategy() }
        };

        public CommandStrategies<IRestScalar> ForScalars { get; set; } = new CommandStrategies<IRestScalar>
        {
            { UnsafeMethod.Put, new ReplaceScalarStrategy() },
            { UnsafeMethod.Delete, new SetScalarToNullStrategy() }
        };
    }
}