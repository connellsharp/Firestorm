namespace Firestorm.Fluent
{
    public interface IFluentCollectionCreatorCreator
    {
        IFluentCollectionCreator GetCollectionCreator<TItem>(ApiItemBuilder<TItem> apiItemBuilder)
            where TItem : class;
    }
}