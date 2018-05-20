using System;

namespace Firestorm.Fluent
{
    public static class ApiItemBuilderExtensions
    {
        public static IApiItemBuilder<TItem> Configure<TItem>(this IApiItemBuilder<TItem> builder, Action<IApiItemBuilder<TItem>> configureAction)
        {
            configureAction(builder);
            return builder;
        }

        public static IApiItemBuilder<TItem> AutoConfigure<TItem>(this IApiItemBuilder<TItem> builder)
        {
            return builder.AutoConfigure(null);
        }

        public static IApiItemBuilder<TItem> AutoConfigure<TItem>(this IApiItemBuilder<TItem> builder, AutoConfiguration configuration)
        {
            if(configuration == null)
                configuration = new AutoConfiguration();
            
            var configurer = new AutoConfigurer(configuration);
            configurer.AddItem<TItem>(builder);

            return builder;
        }
    }
}