using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Firestorm.Fluent
{
    public static class ApiItemBuilderExtensions
    {
        public static IApiItemBuilder<TNavItem> IsItem<TItem, TNavItem>(this IApiFieldBuilder<TItem, TNavItem> builder)
            where TNavItem : class, new()
        {
            return builder.IsItem<TNavItem>();
        }

        public static IApiItemBuilder<TNavItem> IsCollection<TItem, TNavItem>(this IApiFieldBuilder<TItem, IQueryable<TNavItem>> builder)
            where TNavItem : class, new()
        {
            return builder.IsCollection<IQueryable<TNavItem>, TNavItem>();
        }

        public static IApiItemBuilder<TNavItem> IsCollection<TItem, TNavItem>(this IApiFieldBuilder<TItem, ICollection<TNavItem>> builder)
            where TNavItem : class, new()
        {
            return builder.IsCollection<ICollection<TNavItem>, TNavItem>();
        }

        public static IApiItemBuilder<TNavItem> IsCollection<TItem, TNavItem>(this IApiFieldBuilder<TItem, IEnumerable<TNavItem>> builder)
            where TNavItem : class, new()
        {
            return builder.IsCollection<IEnumerable<TNavItem>, TNavItem>();
        }

        public static IApiFieldBuilder<TItem, IQueryable<TNavItem>> IsCollection<TItem, TNavItem>(this IApiFieldBuilder<TItem, IQueryable<TNavItem>> builder, [NotNull] Action<IApiItemBuilder<TNavItem>> buildAction)
            where TNavItem : class, new()
        {
            return builder.IsCollection<TItem, IQueryable<TNavItem>, TNavItem>(buildAction);
        }

        public static IApiFieldBuilder<TItem, ICollection<TNavItem>> IsCollection<TItem, TNavItem>(this IApiFieldBuilder<TItem, ICollection<TNavItem>> builder, [NotNull] Action<IApiItemBuilder<TNavItem>> buildAction)
            where TNavItem : class, new()
        {
            return builder.IsCollection<TItem, ICollection<TNavItem>, TNavItem>(buildAction);
        }

        public static IApiFieldBuilder<TItem, IEnumerable<TNavItem>> IsCollection<TItem, TNavItem>(this IApiFieldBuilder<TItem, IEnumerable<TNavItem>> builder, [NotNull] Action<IApiItemBuilder<TNavItem>> buildAction)
            where TNavItem : class, new()
        {
            return builder.IsCollection<TItem, IEnumerable<TNavItem>, TNavItem>(buildAction);
        }

        public static IApiFieldBuilder<TItem, TCollection> IsCollection<TItem, TCollection, TNavItem>(this IApiFieldBuilder<TItem, TCollection> builder, [NotNull] Action<IApiItemBuilder<TNavItem>> buildAction)
            where TCollection : IEnumerable<TNavItem>
            where TNavItem : class, new()
        {
            IApiItemBuilder<TNavItem> apiItemBuilder = builder.IsCollection<TCollection, TNavItem>();
            buildAction(apiItemBuilder);

            return builder;
        }
    }
}