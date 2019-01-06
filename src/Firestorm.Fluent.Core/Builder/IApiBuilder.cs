using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent
{
    /// <summary>
    /// Provides a simple API for configuring your RESTful API.
    /// </summary>
    public interface IApiBuilder
    {
        IApiItemBuilder<TItem> Item<TItem>()
            where TItem : class, new();

        IApiDirectorySource BuildSource();
    }
}