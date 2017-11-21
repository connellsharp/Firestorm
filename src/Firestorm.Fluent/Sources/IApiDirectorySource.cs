using System.Collections.Generic;

namespace Firestorm.Fluent
{
    public interface IApiDirectorySource
    {
        IRestCollectionSource GetCollectionSource(string collectionName);
    }
}