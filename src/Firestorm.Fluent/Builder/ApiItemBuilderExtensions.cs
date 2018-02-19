using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Fluent.Sources;

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

            Type itemType = typeof(TItem);

            foreach (PropertyInfo property in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                ParameterExpression paramExpression = Expression.Parameter(itemType);
                LambdaExpression expression = Expression.Lambda(Expression.Property(paramExpression, property), paramExpression);

                InvokePrivateStatic(nameof(AddField), new[] { typeof(TItem), property.PropertyType }, builder, expression, configuration);
            }

            return builder;
        }

        private static IApiItemBuilder<TItem> AddField<TItem, TField>(IApiItemBuilder<TItem> builder, Expression<Func<TItem, TField>> expression, AutoConfiguration configuration)
        {
            IApiFieldBuilder<TItem, TField> fieldBuilder = builder.Field(expression);

            if (configuration.AllowWrite)
                fieldBuilder.AllowWrite();

            if (typeof(TField).GetConstructor(new Type[0]) != null)
                InvokePrivateStatic(nameof(AddFieldAsItem), new[] { typeof(TItem), typeof(TField) }, configuration, fieldBuilder);

            Type enumNav = typeof(TField).GetGenericInterface(typeof(IEnumerable<>))?.GetGenericArguments()[0];
            if (enumNav != null)
                InvokePrivateStatic(nameof(AddFieldAsCollection), new[] { typeof(TItem), typeof(TField), enumNav }, configuration, fieldBuilder);

            return builder;
        }

        private static void AddFieldAsItem<TItem, TField>(AutoConfiguration configuration, IApiFieldBuilder<TItem, TField> fieldBuilder)
            where TField : class, new()
        {
            fieldBuilder.IsItem<TField>().AutoConfigure(configuration);
        }

        private static void AddFieldAsCollection<TItem, TField, TNav>(AutoConfiguration configuration, IApiFieldBuilder<TItem, TField> fieldBuilder)
            where TField : class, IEnumerable<TNav>
            where TNav : class, new()
        {
            fieldBuilder.IsCollection<TField, TNav>().AutoConfigure(configuration);
        }

        private static void InvokePrivateStatic(string methodName, Type[] types, params object[] args)
        {
            MethodInfo method = typeof(ApiItemBuilderExtensions).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo genericMethod = method?.MakeGenericMethod(types);
            Debug.Assert(genericMethod != null, "Couldn't find generic method with reflection.");
            genericMethod.Invoke(null, args);
        }
    }
}