using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Reflectious;

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
            this.Reflect()
                .GetMethod(nameof(AddRootItem))
                .MakeGeneric(itemType)
                .Invoke(builder, rootName);
        }

        public void AddItem<TItem>(IApiItemBuilder<TItem> builder)
        {
            AddItem(builder, 0);
        }

        private void AddRootItem<TItem>(IApiBuilder builder, [CanBeNull] string rootName)
            where TItem : class, new()
        {
            var itemBuilder = builder.Item<TItem>();
            AddItem(itemBuilder);

            if (!string.IsNullOrEmpty(rootName))
                itemBuilder.RootName = rootName;
        }
        
        private void AddItem<TItem>(IApiItemBuilder<TItem> builder, int nesting)
        {
            Type itemType = typeof(TItem);

            foreach (PropertyInfo property in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                ParameterExpression paramExpression = Expression.Parameter(itemType);
                LambdaExpression expression = Expression.Lambda(Expression.Property(paramExpression, property), paramExpression);

                this.Reflect()
                    .GetMethod(nameof(AddField))
                    .MakeGeneric(typeof(TItem), property.PropertyType)
                    .Invoke(builder, expression, nesting);
            }
        }

        private IApiItemBuilder<TItem> AddField<TItem, TField>(IApiItemBuilder<TItem> builder, Expression<Func<TItem, TField>> expression, int nesting)
        {
            IApiFieldBuilder<TItem, TField> fieldBuilder = builder.Field(expression);

            if (_configuration.AllowWrite)
                fieldBuilder.AllowWrite();

            if (nesting >= _configuration.MaxNesting)
                return builder;

            if (typeof(TField).GetConstructor(new Type[0]) != null)
            {
                this.Reflect()
                    .GetMethod(nameof(AddFieldAsItem))
                    .MakeGeneric<TItem, TField>()
                    .Invoke(fieldBuilder, nesting);
            }

            Type enumNav = typeof(TField).GetGenericInterface(typeof(ICollection<>))?.GetGenericArguments()[0];
            if (enumNav != null)
            {
                this.Reflect()
                    .GetMethod(nameof(AddFieldAsCollection))
                    .MakeGeneric(typeof(TItem), typeof(TField), enumNav)
                    .Invoke(fieldBuilder, nesting);
            }

            return builder;
        }

        private void AddFieldAsItem<TItem, TField>(IApiFieldBuilder<TItem, TField> fieldBuilder, int nesting)
            where TField : class, new()
        {
            var subItemBuilder = fieldBuilder.IsItem<TField>();
            AddItem<TField>(subItemBuilder, nesting + 1);
        }

        private void AddFieldAsCollection<TItem, TField, TNav>(IApiFieldBuilder<TItem, TField> fieldBuilder, int nesting)
            where TField : class, IEnumerable<TNav>
            where TNav : class, new()
        {
            var subItemBuilder = fieldBuilder.IsCollection<TField, TNav>();
            AddItem<TNav>(subItemBuilder, nesting + 1);
        }
    }
}