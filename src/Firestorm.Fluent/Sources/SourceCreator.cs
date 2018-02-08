using System;
using System.Reflection;

namespace Firestorm.Fluent.Sources
{
    public class SourceCreator
    {
        public IApiDirectorySource CreateSource(ApiContext apiContext, IApiBuilder builder)
        {
            AddApiRootProperties(apiContext, builder);

            apiContext.OnApiCreating(builder);

            return builder.BuildSource();
        }

        private static void AddApiRootProperties(ApiContext apiContext, IApiBuilder builder)
        {
            foreach (PropertyInfo property in apiContext.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Type rootType = property.PropertyType.GetGenericSubclass(typeof(ApiRoot<>));
                if (rootType == null)
                    continue;

                Type itemType = rootType.GetGenericArguments()[0];

                MethodInfo addRootMethod = typeof(SourceCreator).GetMethod(nameof(AddRootItem), BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(itemType);
                addRootMethod.Invoke(null, new object[] { builder, property.Name, apiContext.Options.RootConfiguration });
            }
        }

        private static void AddRootItem<TItem>(IApiBuilder builder, string rootName, AutoConfiguration configuration)
            where TItem : class, new()
        {
            var itemBuilder = builder.Item<TItem>().AutoConfigure(configuration);
            itemBuilder.RootName = rootName;
        }
    }
}
