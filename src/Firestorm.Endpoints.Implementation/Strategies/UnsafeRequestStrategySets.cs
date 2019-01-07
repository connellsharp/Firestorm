using Firestorm.Endpoints.Requests;

namespace Firestorm.Endpoints.Strategies
{
    /// <summary>
    /// Contains 3 sets of strategies (for collection, items and scalars) defining how endpoints behave to unsafe requests.
    /// </summary>
    public class UnsafeRequestStrategySets : IUnsafeRequestStrategySets
    {
        public UnsafeRequestStrategies<IRestCollection> ForCollections { get; set; } = new UnsafeRequestStrategies<IRestCollection>
        {
            { UnsafeMethod.Post, new AddToCollectionStrategy() },
            //{ UnsafeMethod.Delete, new CollectionFilteredDeleteStrategy() }
        };

        public UnsafeRequestStrategies<IRestItem> ForItems { get; set; } = new UnsafeRequestStrategies<IRestItem>
        {
            { UnsafeMethod.Put, new UpdateItemStrategy() },
            { UnsafeMethod.Delete, new DeleteItemStrategy() }
        };

        public UnsafeRequestStrategies<IRestScalar> ForScalars { get; set; } = new UnsafeRequestStrategies<IRestScalar>
        {
            { UnsafeMethod.Put, new ReplaceScalarStrategy() },
            { UnsafeMethod.Delete, new SetScalarToNullStrategy() }
        };
    }
}