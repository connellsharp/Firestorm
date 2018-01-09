using System;
using System.Linq.Expressions;

namespace Firestorm.Fluent
{
    public interface IApiItemBuilder
    {
        string RootName { set; }
    }

    public interface IApiItemBuilder<TItem> : IApiItemBuilder
    {
        IApiIdentifierBuilder<TItem, TIdentifier> Identifier<TIdentifier>(Expression<Func<TItem, TIdentifier>> expression);

        IApiFieldBuilder<TItem, TField> Field<TField>(Expression<Func<TItem, TField>> expression);

        IApiItemBuilder<TItem> OnCreating(Action<TItem> expression);
    }
}