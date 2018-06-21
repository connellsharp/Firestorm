using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Firestorm.Fluent
{
    public static class ApiFieldBuilderExtensions
    {
        public static IApiItemBuilder<TNavItem> IsItem<TItem, TNavItem>(this IApiFieldBuilder<TItem, TNavItem> builder)
            where TNavItem : class, new()
        {
            return builder.IsItem<TNavItem>();
        }

        public static IApiFieldBuilder<TItem, TNavItem> IsItem<TItem, TNavItem>(this IApiFieldBuilder<TItem, TNavItem> builder, [NotNull] Action<IApiItemBuilder<TNavItem>> configureAction)
            where TNavItem : class, new()
        {
            builder.IsItem<TNavItem>()
                .Configure(configureAction);

            return builder;
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

        public static IApiFieldBuilder<TItem, IQueryable<TNavItem>> IsCollection<TItem, TNavItem>(this IApiFieldBuilder<TItem, IQueryable<TNavItem>> builder, [NotNull] Action<IApiItemBuilder<TNavItem>> configureAction)
            where TNavItem : class, new()
        {
            return builder.IsCollection<TItem, IQueryable<TNavItem>, TNavItem>(configureAction);
        }

        public static IApiFieldBuilder<TItem, ICollection<TNavItem>> IsCollection<TItem, TNavItem>(this IApiFieldBuilder<TItem, ICollection<TNavItem>> builder, [NotNull] Action<IApiItemBuilder<TNavItem>> configureAction)
            where TNavItem : class, new()
        {
            return builder.IsCollection<TItem, ICollection<TNavItem>, TNavItem>(configureAction);
        }

        public static IApiFieldBuilder<TItem, IList<TNavItem>> IsCollection<TItem, TNavItem>(this IApiFieldBuilder<TItem, IList<TNavItem>> builder, [NotNull] Action<IApiItemBuilder<TNavItem>> configureAction)
            where TNavItem : class, new()
        {
            return builder.IsCollection<TItem, IList<TNavItem>, TNavItem>(configureAction);
        }

        public static IApiFieldBuilder<TItem, IEnumerable<TNavItem>> IsCollection<TItem, TNavItem>(this IApiFieldBuilder<TItem, IEnumerable<TNavItem>> builder, [NotNull] Action<IApiItemBuilder<TNavItem>> configureAction)
            where TNavItem : class, new()
        {
            return builder.IsCollection<TItem, IEnumerable<TNavItem>, TNavItem>(configureAction);
        }

        public static IApiFieldBuilder<TItem, TCollection> IsCollection<TItem, TCollection, TNavItem>(this IApiFieldBuilder<TItem, TCollection> builder, [NotNull] Action<IApiItemBuilder<TNavItem>> configureAction)
            where TCollection : class, IEnumerable<TNavItem>
            where TNavItem : class, new()
        {
            builder.IsCollection<TCollection, TNavItem>()
                .Configure(configureAction);

            return builder;
        }
    }
}