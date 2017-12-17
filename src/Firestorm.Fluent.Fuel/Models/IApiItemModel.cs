using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent.Fuel.Models
{
    public interface IApiItemModel
    {
        string RootName { get; }

        IRestCollectionSource GetCollectionSource();
    }
}