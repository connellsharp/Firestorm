using System.Collections.Generic;

namespace Firestorm.Fluent
{
    public interface IApiFieldBuilder<TItem>
    { }

    public interface IApiFieldBuilder<TItem, TField> : IApiFieldBuilder<TItem>
    {
        IApiFieldBuilder<TItem, TField> HasName(string fieldName);

        IApiFieldBuilder<TItem, TField> AllowWrite();

        IApiItemBuilder<TNavItem> IsItem<TNavItem>()
            where TNavItem : class, TField, new();

        IApiItemBuilder<TNavItem> IsCollection<TCollection, TNavItem>()
            where TCollection : TField, IEnumerable<TNavItem>
            where TNavItem : class, new();
    }
}