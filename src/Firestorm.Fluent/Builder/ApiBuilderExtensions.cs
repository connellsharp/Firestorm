using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Firestorm.Fluent
{
    public static class ApiBuilderExtensions
    {
        public static IApiBuilder Item<TItem>(this IApiBuilder builder, [NotNull] Action<IApiItemBuilder<TItem>> configureAction)
            where TItem : class, new()
        {
            builder.Item<TItem>()
                .Configure(configureAction);

            return builder;
        }
    }
}