namespace Firestorm.Fluent
{
    public class ApiCollectionSource
    {
        public string Name { get; internal set; }

        public IFluentCollectionCreator CollectionCreator { get; internal set; }
    }
}