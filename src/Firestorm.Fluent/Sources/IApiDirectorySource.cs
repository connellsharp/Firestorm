namespace Firestorm.Fluent.Sources
{
    public interface IApiDirectorySource
    {
        IRestCollectionSource GetCollectionSource(string collectionName);
    }
}