using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Firestorm.Fluent
{
    public static class ApiItemBuilderExtensions
    {
        public static IApiItemBuilder<TItem> AutoConfigure<TItem>(this IApiItemBuilder<TItem> builder)
        {
            return builder.AutoConfigure(null);
        }

        public static IApiItemBuilder<TItem> AutoConfigure<TItem>(this IApiItemBuilder<TItem> builder, AutoConfiguration configuration)
        {
            if(configuration == null)
                configuration = new AutoConfiguration();

            Type itemType = typeof(TItem);

            foreach (PropertyInfo property in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                ParameterExpression paramExpression = Expression.Parameter(itemType);
                LambdaExpression expression = Expression.Lambda(Expression.Property(paramExpression, property), paramExpression);

                MethodInfo method = typeof(ApiItemBuilderExtensions).GetMethod(nameof(AddField), BindingFlags.NonPublic | BindingFlags.Static);
                MethodInfo genericMethod = method.MakeGenericMethod(typeof(TItem), property.PropertyType);
                Debug.Assert(genericMethod != null, "Couldn't find generic method to add field with reflection.");
                genericMethod.Invoke(null, new object[] { builder, expression, configuration });
            }

            return builder;
        }

        private static IApiItemBuilder<TItem> AddField<TItem, TField>(IApiItemBuilder<TItem> builder, Expression<Func<TItem, TField>> expression, AutoConfiguration configuration)
        {
            var fieldBuilder = builder.Field(expression);

            if (configuration.AllowWrite)
                fieldBuilder.AllowWrite();

            // TODO auto configure sub items and collections
            //fieldBuilder.IsItem<TField>().AutoConfigure(configuration);
            //fieldBuilder.IsCollection<IEnumerable<TField>, TField>().AutoConfigure(configuration);

            return builder;
        }

        public static IApiItemBuilder<TItem> Configure<TItem>(this IApiItemBuilder<TItem> builder, Action<IApiItemBuilder<TItem>> configureAction)
        {
            configureAction(builder);
            return builder;
        }
    }
}