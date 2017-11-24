using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine.Queryable
{
    internal class QueryableFieldSelector<TItem>
        where TItem : class
    {
        private readonly IDictionary<string, IFieldReader<TItem>> _fieldReaders;

        public QueryableFieldSelector([NotNull] IFieldProvider<TItem> fieldProvider, [NotNull] IEnumerable<string> fieldNames)
            : this(fieldProvider.GetReaders(fieldNames))
        { }

        public QueryableFieldSelector([NotNull] IDictionary<string, IFieldReader<TItem>> namedReaders)
        {
            _fieldReaders = namedReaders ?? throw new ArgumentNullException(nameof(namedReaders));
        }

        public async Task<RestItemData> SelectFieldsOnlyAsync(TItem item)
        {
            Type dynamicType = GetDynamicRuntimeType();
            object dynamicObj =  GetDynamicObject(item, dynamicType);

            IQueryable<TItem> items = new[] { item }.AsQueryable();
            var replacerDictionary = await LoadAllReplacersAsync(items);
            ReplaceWithDictionary(replacerDictionary, dynamicObj, dynamicType);

            return new RestItemData(dynamicObj);
        }

        /// <summary>
        /// Executes a query that selects only the desired fields from an <see cref="IQueryable{TItem}"/>.
        /// </summary>
        public async Task<IEnumerable<RestItemData>> SelectFieldsOnlyAsync(IQueryable<TItem> items, ForEachAsyncDelegate<object> forEachAsync)
        {
            Type dynamicType = GetDynamicRuntimeType();
            IQueryable dynamicQueryable = GetDynamicQueryable(items, dynamicType);

            var replacerDictionary = await LoadAllReplacersAsync(items);
            IEnumerable dynamicEnumerable = await ExecuteWithReplacementsAsync(replacerDictionary, dynamicQueryable, forEachAsync);

            return GetRestItemDataIterator(dynamicEnumerable);
        }

        private Type GetDynamicRuntimeType()
        {
            var dynamicFields = _fieldReaders.Select(fm => new KeyValuePair<string, Type>(fm.Key, fm.Value.FieldType));
            return LinqRuntimeTypeBuilder.GetDynamicType(dynamicFields);
        }

        /// <remarks>
        /// Idea from http://stackoverflow.com/a/723018/369247
        /// </remarks>
        private IQueryable GetDynamicQueryable(IQueryable<TItem> items, Type dynamicType)
        {
            ParameterExpression itemPram = Expression.Parameter(typeof(TItem), "itm");

            var initExpressionBuilder = new MemberInitExpressionBuilder(dynamicType);
            MemberInitExpression memberInitExpr = initExpressionBuilder.Build(itemPram, _fieldReaders);
            IQueryable selectDynamicQuery = ExpressionTreeHelpers.GetSelectByExpressionQuery(items, itemPram, memberInitExpr);
            return selectDynamicQuery;
        }

        private object GetDynamicObject(TItem item, Type dynamicType)
        {
            ParameterExpression itemPram = Expression.Parameter(typeof(TItem), "itm");
            object dynamicObj = Activator.CreateInstance(dynamicType, new object[0]);

            foreach (var fieldReader in _fieldReaders)
            {
                Expression getterExpr = fieldReader.Value.GetSelectExpression(itemPram);
                LambdaExpression lambda = Expression.Lambda(getterExpr, itemPram);
                object value = lambda.Compile().DynamicInvoke(item);

                FieldInfo fieldInfo = dynamicType.GetField(fieldReader.Key);
                fieldInfo.SetValue(dynamicObj, value);
            }

            return dynamicObj;
        }

        private async Task<IDictionary<string, IFieldValueReplacer<TItem>>> LoadAllReplacersAsync(IQueryable<TItem> items)
        {
            var replacerDictionary = new ConcurrentDictionary<string, IFieldValueReplacer<TItem>>();
            var tasks = new List<Task>();

            foreach (var fieldReader in _fieldReaders)
            {
                IFieldValueReplacer<TItem> replacer = fieldReader.Value.Replacer;
                if (replacer == null)
                    continue;

                Task preloadTask = replacer.LoadAsync(items).ContinueWith(task =>
                {
                    if (!replacerDictionary.TryAdd(fieldReader.Key, replacer))
                        throw new InvalidOperationException("Error adding reader replacer to dictionary.");
                });

                tasks.Add(preloadTask);
            }

            await Task.WhenAll(tasks);

            return replacerDictionary;
        }

        private async Task<IEnumerable> ExecuteWithReplacementsAsync(IDictionary<string, IFieldValueReplacer<TItem>> replacerDictionary, IQueryable dynamicQueryable, ForEachAsyncDelegate<object> forEachAsync)
        {
            var dynamicType = dynamicQueryable.ElementType;
            var returnObjects = new List<object>();
            
            if (dynamicQueryable.IsInMemory())
                await ItemQueryHelper.DefaultForEachAsync(dynamicQueryable.OfType<object>(), AddObjectToList);
            else
                await forEachAsync(dynamicQueryable.AsObjects(), AddObjectToList);

            void AddObjectToList(object dynamicObj)
            {
                ReplaceWithDictionary(replacerDictionary, dynamicObj, dynamicType);
                returnObjects.Add(dynamicObj);
            }

            return returnObjects;
        }

        private static void ReplaceWithDictionary(IDictionary<string, IFieldValueReplacer<TItem>> replacerDictionary, object dynamicObj, Type dynamicType)
        {
            foreach (var replacer in replacerDictionary)
            {
                Debug.Assert(replacer.Value != null, "Field value should not be in the preloaded list if there is no replacer.");

                FieldInfo fieldInfo = dynamicType.GetField(replacer.Key);

                object dbValue = fieldInfo.GetValue(dynamicObj);
                object replacementValue = replacer.Value.GetReplacement(dbValue);

                fieldInfo.SetValue(dynamicObj, replacementValue);
            }
        }

        private static IEnumerable<RestItemData> GetRestItemDataIterator(IEnumerable dynamicEnumerable)
        {
            foreach (object obj in dynamicEnumerable)
            {
                yield return new RestItemData(obj);
            }
        }
    }
}