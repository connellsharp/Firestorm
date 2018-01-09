using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent.Fuel.Models
{
    public interface IApiRoot
    {
        string RootName { get; }
        
        IRestCollectionSource GetRootCollectionSource();
    }
}