using System;

namespace Firestorm.Fluent
{
    public interface IApiItemBuilder
    {
        IFluentCollectionCreator GetCollectionCreator(IFluentCollectionCreatorCreator fluentCreator);
        Type ItemType { get; }
    }
}