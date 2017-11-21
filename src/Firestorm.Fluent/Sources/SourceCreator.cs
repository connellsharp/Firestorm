using System.Collections.Generic;

namespace Firestorm.Fluent
{
    public class SourceCreator
    {
        public ApiDirectorySource CreateSource(ApiContext apiContext, IFluentCollectionCreatorCreator creatorCreator)
        {
            var builder = new ApiBuilder();
            apiContext.OnModelCreating(builder);
            return new ApiDirectorySource(builder, creatorCreator);
        }
    }

    public interface IFluentCollectionCreatorCreator
    {
        IFluentCollectionCreator GetCollectionCreator<TItem>(ApiItemBuilder<TItem> apiItemBuilder)
            where TItem : class;
    }

    public class ApiDirectorySource
    {
        private readonly ApiBuilder _builder;
        private readonly IFluentCollectionCreatorCreator _fluentCreator;

        internal ApiDirectorySource(ApiBuilder builder, IFluentCollectionCreatorCreator fluentCreator)
        {
            _builder = builder;
            _fluentCreator = fluentCreator;
        }

        public IEnumerable<ApiCollectionSource> GetRootSources()
        {
            foreach (IApiItemBuilder itemBuilder in _builder.Items)
            {
                yield return new ApiCollectionSource
                {
                    Name = itemBuilder.ItemType.Name,
                    CollectionCreator = itemBuilder.GetCollectionCreator(_fluentCreator)
                };
            }
        }
    }

    public class ApiCollectionSource
    {
        public string Name { get; internal set; }

        public IFluentCollectionCreator CollectionCreator { get; internal set; }
    }
}
