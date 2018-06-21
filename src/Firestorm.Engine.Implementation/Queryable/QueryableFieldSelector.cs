using System;
using System.Collections.Generic;
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

            if (dynamicObj == null)
                return null;

            IQueryable<TItem> items = new[] { item }.AsQueryable();
            var replacementProcessor = new FieldReplacementProcessor<TItem>(_fieldReaders);
            await replacementProcessor.LoadAllAsync(items);
            replacementProcessor.Replace(dynamicObj, dynamicType);

            return new RestItemData(dynamicObj);
        }

        /// <summary>
        /// Executes a query that selects only the desired fields from an <see cref="IQueryable{TItem}"/>.
        /// </summary>
        public async Task<QueriedDataIterator> SelectFieldsOnlyAsync(IQueryable<TItem> items, ForEachAsyncDelegate<object> forEachAsync)
        {
            Type dynamicType = GetDynamicRuntimeType();
            IQueryable dynamicQueryable = GetDynamicQueryable(items, dynamicType);

            var replacementProcessor = new FieldReplacementProcessor<TItem>(_fieldReaders);
            await replacementProcessor.LoadAllAsync(items);
            List<object> dynamicObjects = await ExecuteWithReplacementsAsync(replacementProcessor, dynamicQueryable, forEachAsync);

            return new QueriedDataIterator(dynamicObjects);
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
            
            Expression nullCondition = ExpressionTreeHelpers.NullConditional(memberInitExpr, itemPram);

            IQueryable selectDynamicQuery = ExpressionTreeHelpers.GetSelectExpressionQuery(items, itemPram, nullCondition);
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

        private static async Task<List<object>> ExecuteWithReplacementsAsync(FieldReplacementProcessor<TItem> replacementProcessor, IQueryable dynamicQueryable, ForEachAsyncDelegate<object> forEachAsync)
        {
            var dynamicType = dynamicQueryable.ElementType;
            var returnObjects = new List<object>();
            
            if (dynamicQueryable.IsInMemory())
                await ItemQueryHelper.DefaultForEachAsync(dynamicQueryable, AddObjectToList);
            else
                await forEachAsync(dynamicQueryable.AsObjects(), AddObjectToList);

            void AddObjectToList(object dynamicObj)
            {
                if (dynamicObj != null)
                    replacementProcessor.Replace(dynamicObj, dynamicType);

                returnObjects.Add(dynamicObj);
            }

            return returnObjects;
        }
    }
}