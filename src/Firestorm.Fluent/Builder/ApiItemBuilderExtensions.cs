using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Firestorm.Fluent
{
    public static class ApiItemBuilderExtensions
    {
        public static IApiItemBuilder<TItem> AutoConfigure<TItem>(this IApiItemBuilder<TItem> builder)
        {
            Type itemType = typeof(TItem);

            foreach (PropertyInfo property in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                ParameterExpression paramExpression = Expression.Parameter(itemType);
                LambdaExpression expression = Expression.Lambda(Expression.Property(paramExpression, property), paramExpression);

                MethodInfo method = typeof(IApiItemBuilder<TItem>).GetMethod(nameof(IApiItemBuilder<TItem>.Field), BindingFlags.Public | BindingFlags.Instance);
                MethodInfo genericMethod = method.MakeGenericMethod(property.PropertyType);
                genericMethod.Invoke(builder, new object[] { expression });
            }

            return builder;
        }

        public static IApiItemBuilder<TItem> Configure<TItem>(this IApiItemBuilder<TItem> builder, Action<IApiItemBuilder<TItem>> configureAction)
        {
            configureAction(builder);
            return builder;
        }
    }
}