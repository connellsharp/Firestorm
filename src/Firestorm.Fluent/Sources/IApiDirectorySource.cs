using System.Collections.Generic;

namespace Firestorm.Fluent.Sources
{
    public interface IApiDirectorySource
    {
        IRestCollectionSource GetCollectionSource(string collectionName);

        IEnumerable<string> GetCollectionNames();
    }
}