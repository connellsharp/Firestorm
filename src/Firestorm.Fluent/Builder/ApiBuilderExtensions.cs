using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Firestorm.Fluent
{
    public static class ApiBuilderExtensions
    {
        public static IApiBuilder Item<TItem>(this IApiBuilder builder, [NotNull] Action<IApiItemBuilder<TItem>> buildAction)
            where TItem : class
        {
            buildAction(builder.Item<TItem>());
            return builder;
        }
    }
}