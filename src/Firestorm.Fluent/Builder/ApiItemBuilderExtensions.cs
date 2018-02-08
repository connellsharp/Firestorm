using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Firestorm.Fluent
{
    public static class ApiItemBuilderExtensions
    {
        public static IApiItemBuilder<TItem> AutoConfigure<TItem>(this IApiItemBuilder<TItem> builder, AutoConfigureMode mode)
        {
            Type itemType = typeof(TItem);

            foreach (PropertyInfo property in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                ParameterExpression paramExpression = Expression.Parameter(itemType);
                LambdaExpression expression = Expression.Lambda(Expression.Property(paramExpression, property), paramExpression);

                MethodInfo method = typeof(IApiItemBuilder<TItem>).GetMethod(nameof(AddField), BindingFlags.NonPublic | BindingFlags.Static);
                MethodInfo genericMethod = method.MakeGenericMethod(property.PropertyType);
                genericMethod.Invoke(null, new object[] { builder, expression, mode });
            }

            return builder;
        }

        private static IApiItemBuilder<TItem> AddField<TItem, TField>(IApiItemBuilder<TItem> builder, Expression<Func<TItem, TField>> expression, AutoConfigureMode mode)
        {
            var fieldBuilder = builder.Field(expression);

            if (mode == AutoConfigureMode.ReadWrite)
                fieldBuilder.AllowWrite();

            return builder;
        }

        public static IApiItemBuilder<TItem> Configure<TItem>(this IApiItemBuilder<TItem> builder, Action<IApiItemBuilder<TItem>> configureAction)
        {
            configureAction(builder);
            return builder;
        }
    }
}