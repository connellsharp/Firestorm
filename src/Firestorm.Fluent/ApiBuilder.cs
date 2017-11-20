using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Firestorm.Fluent
{
    public class ApiBuilder
    {
        internal ApiBuilder()
        {
        }

        public virtual ApiItemBuilder<TItem> Item<TItem>()
            where TItem : class
        {
            var itemBuilder = new ApiItemBuilder<TItem>();
            Items.Add(itemBuilder);
            return itemBuilder;
        }

        internal IList<IApiItemBuilder> Items { get; } = new List<IApiItemBuilder>();

        public virtual ApiBuilder Item<TItem>([NotNull] Action<ApiItemBuilder<TItem>> buildAction)
            where TItem : class
        {
            buildAction(Item<TItem>());
            return this;
        }
    }

    public class ApiItemBuilder<TItem> : IApiItemBuilder
        where TItem : class
    {
        public ApiIdentifierBuilder<TItem> Identifier<TIdentifier>(Expression<Func<TItem, TIdentifier>> expression)
        {
            var identifierBuilder = new ApiIdentifierBuilder<TItem>();
            return identifierBuilder;
        }

        public ApiFieldBuilder<TItem, TField> Field<TField>(Expression<Func<TItem, TField>> expression)
        {
            var fieldBuilder = new ApiFieldBuilder<TItem, TField>(expression);
            Fields.Add(fieldBuilder);
            return fieldBuilder;
        }

        internal IList<IApiFieldBuilder<TItem>> Fields { get; } = new List<IApiFieldBuilder<TItem>>();

        public IApiCollectionSource GetApiSet()
        {
            return new ApiCollectionSource<TItem>();
        }
    }

    public interface IApiItemBuilder
    {
        IApiCollectionSource GetApiSet();
    }

    public class ApiIdentifierBuilder<TItem>
    { }

    public class ApiFieldBuilder<TItem, TField> : IApiFieldBuilder<TItem>
    {
        public Expression<Func<TItem, TField>> Expression { get; }

        internal string Name { get; private set; }

        public ApiFieldBuilder(Expression<Func<TItem, TField>> expression)
        {
            Expression = expression;
        }

        public ApiFieldBuilder<TItem, TField> HasName(string fieldName)
        {
            Name = fieldName;
            return this;
        }
    }

    public interface IApiFieldBuilder<TItem>
    { }
}