using Firestorm.Fluent.Sources;

namespace Firestorm.Fluent
{
    public interface IApiBuilder
    {
        IApiItemBuilder<TItem> Item<TItem>()
            where TItem : class;

        IApiDirectorySource BuildSource();
    }
}