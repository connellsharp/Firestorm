using System.Collections.Generic;

namespace Firestorm.Fluent
{
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
}