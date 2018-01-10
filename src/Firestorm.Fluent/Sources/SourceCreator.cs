using System;
using System.Reflection;

namespace Firestorm.Fluent.Sources
{
    public class SourceCreator
    {
        public IApiDirectorySource CreateSource(RestContext restContext, IApiBuilder builder)
        {
            AddApiRootProperties(restContext, builder);

            restContext.OnApiCreating(builder);

            return builder.BuildSource();
        }

        private static void AddApiRootProperties(RestContext restContext, IApiBuilder builder)
        {
            foreach (PropertyInfo property in restContext.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Type rootType = property.PropertyType.GetGenericSubclass(typeof(ApiRoot<>));
                if (rootType == null)
                    continue;

                Type itemType = rootType.GetGenericArguments()[0];

                MethodInfo addRootMethod = typeof(SourceCreator).GetMethod(nameof(AddRootItem), BindingFlags.NonPublic | BindingFlags.Static)?.MakeGenericMethod(itemType);
                addRootMethod.Invoke(null, new object[] { builder, property.Name });
            }
        }

        private static void AddRootItem<TItem>(IApiBuilder builder, string rootName)
            where TItem : class, new()
        {
            var itemBuilder = builder.Item<TItem>().AutoConfigure();
            itemBuilder.RootName = rootName;
        }
    }
}
