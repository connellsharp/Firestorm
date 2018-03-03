using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Fluent.Sources;
using JetBrains.Annotations;

namespace Firestorm.Fluent
{
    internal class AutoConfigurer
    {
        private readonly AutoConfiguration _configuration;

        public AutoConfigurer(AutoConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AddApiRootProperties(IApiBuilder builder, Type contextType)
        {
            foreach (PropertyInfo property in contextType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Type rootType = property.PropertyType.GetGenericSubclass(typeof(ApiRoot<>));
                if (rootType == null)
                    continue;

                Type itemType = rootType.GetGenericArguments()[0];
                AddRootItem(builder, itemType, property.Name);
            }
        }

        public void AddRootItem(IApiBuilder builder, Type itemType, [CanBeNull] string rootName)
        {
            InvokePrivate(nameof(AddRootItem), new[] {itemType}, builder, rootName);
        }

        private void AddRootItem<TItem>(IApiBuilder builder, [CanBeNull] string rootName)
            where TItem : class, new()
        {
            var itemBuilder = builder.Item<TItem>();
            AddItem(itemBuilder);

            if (!string.IsNullOrEmpty(rootName))
                itemBuilder.RootName = rootName;
        }
        
        public void AddItem<TItem>(IApiItemBuilder<TItem> builder)
        {
            Type itemType = typeof(TItem);

            foreach (PropertyInfo property in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                ParameterExpression paramExpression = Expression.Parameter(itemType);
                LambdaExpression expression = Expression.Lambda(Expression.Property(paramExpression, property), paramExpression);

                InvokePrivate(nameof(AddField), new[] { typeof(TItem), property.PropertyType }, builder, expression);
            }
        }

        private IApiItemBuilder<TItem> AddField<TItem, TField>(IApiItemBuilder<TItem> builder, Expression<Func<TItem, TField>> expression)
        {
            IApiFieldBuilder<TItem, TField> fieldBuilder = builder.Field(expression);

            if (_configuration.AllowWrite)
                fieldBuilder.AllowWrite();

            if (typeof(TField).GetConstructor(new Type[0]) != null)
                InvokePrivate(nameof(AddFieldAsItem), new[] { typeof(TItem), typeof(TField) }, fieldBuilder);

            Type enumNav = typeof(TField).GetGenericInterface(typeof(ICollection<>))?.GetGenericArguments()[0];
            if (enumNav != null)
                InvokePrivate(nameof(AddFieldAsCollection), new[] { typeof(TItem), typeof(TField), enumNav }, fieldBuilder);

            return builder;
        }

        private void AddFieldAsItem<TItem, TField>(IApiFieldBuilder<TItem, TField> fieldBuilder)
            where TField : class, new()
        {
            var subItemBuilder = fieldBuilder.IsItem<TField>();
            AddItem<TField>(subItemBuilder);
        }

        private void AddFieldAsCollection<TItem, TField, TNav>(IApiFieldBuilder<TItem, TField> fieldBuilder)
            where TField : class, IEnumerable<TNav>
            where TNav : class, new()
        {
            var subItemBuilder = fieldBuilder.IsCollection<TField, TNav>();
            AddItem<TNav>(subItemBuilder);
        }

        private void InvokePrivate(string methodName, Type[] types, params object[] args)
        {
            MethodInfo method = typeof(AutoConfigurer).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo genericMethod = method?.MakeGenericMethod(types);
            Debug.Assert(genericMethod != null, "Couldn't find generic method with reflection.");
            genericMethod.Invoke(this, args);
        }

    }
}