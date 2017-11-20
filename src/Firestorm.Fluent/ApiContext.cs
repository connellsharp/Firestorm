using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Firestorm.Fluent
{
    public abstract class ApiContext
    {
        protected internal virtual void OnModelCreating(ApiBuilder apiBuilder)
        {
        }
    }

    public class SourceCreator
    {
        public ApiDirectorySource CreateSource(ApiContext apiContext)
        {
            var builder = new ApiBuilder();
            apiContext.OnModelCreating(builder);
            return new ApiDirectorySource(builder);
        }
    }

    public class ApiDirectorySource
    {
        private readonly ApiBuilder _builder;

        internal ApiDirectorySource(ApiBuilder builder)
        {
            _builder = builder;
        }

        public IEnumerable<IApiCollectionSource> GetRootSources()
        {
            foreach (IApiItemBuilder itemBuilder in _builder.Items)
            {
                yield return itemBuilder.GetApiSet();
            }
        }
    }

    public interface IApiCollectionSource
    {
        string Name { get; }

        IFluentCollectionCreator GetCollectionCreator();
    }

    public class ApiCollectionSource<TItem> : IApiCollectionSource
    {
        public string Name { get; }

        public IFluentCollectionCreator GetCollectionCreator()
        {
            throw new NotImplementedException("Fluent API not implemented yet.");
        }
    }

}
