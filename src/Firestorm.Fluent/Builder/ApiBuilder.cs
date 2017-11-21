using System;
using System.Collections.Generic;
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

        public IList<IApiItemBuilder> Items { get; } = new List<IApiItemBuilder>();

        public virtual ApiBuilder Item<TItem>([NotNull] Action<ApiItemBuilder<TItem>> buildAction)
            where TItem : class
        {
            buildAction(Item<TItem>());
            return this;
        }
    }
}