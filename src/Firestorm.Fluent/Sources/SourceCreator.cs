using System;
using System.Reflection;

namespace Firestorm.Fluent.Sources
{
    public class SourceCreator
    {
        public IApiDirectorySource CreateSource(RestContext restContext, IApiBuilder builder)
        {
            AddRoots(restContext, builder);

            restContext.OnApiCreating(builder);

            return builder.BuildSource();
        }

        private static void AddRoots(RestContext restContext, IApiBuilder builder)
        {
            foreach (PropertyInfo property in restContext.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Type rootType = property.PropertyType.GetGenericSubclass(typeof(ApiRoot<>));
                if (rootType == null)
                    continue;

                Type itemType = rootType.GetGenericArguments()[0];

                MethodInfo addRootMethod = typeof(SourceCreator).GetMethod(nameof(AddRoot), BindingFlags.NonPublic | BindingFlags.Static)?.MakeGenericMethod(itemType);
                addRootMethod.Invoke(null, new object[] { builder });
            }
        }

        private static void AddRoot<TItem>(IApiBuilder builder)
            where TItem : class, new()
        {
            builder.Item<TItem>().AutoConfigure();
        }
    }
}
