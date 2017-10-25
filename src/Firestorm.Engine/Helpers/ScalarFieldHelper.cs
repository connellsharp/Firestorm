using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Engine.Fields;

namespace Firestorm.Engine
{
    public static class ScalarFieldHelper
    {
        public static bool IsScalarType(Type fieldType)
        {
            Type type = Nullable.GetUnderlyingType(fieldType) ?? fieldType;
            TypeCode typeCode = Type.GetTypeCode(type);
            return typeCode != TypeCode.Empty && typeCode != TypeCode.Object;
        }

        /// <summary>
        /// Loads a single scalar field value from a single <see cref="itemQuery"/>.
        /// </summary>
        public static async Task<object> LoadScalarValueAsync<TItem>(IFieldReader<TItem> value, IQueryableSingle<TItem> itemQuery, ForEachAsyncDelegate<object> forEachAsync)
        {
            ParameterExpression itemPram = Expression.Parameter(typeof(TItem), "itm");
            Expression selectExpression = value.GetSelectExpression(itemPram);

            IQueryable selectScalarOnlyQuery = ExpressionTreeHelpers.GetSelectByExpressionQuery(itemQuery, itemPram, selectExpression);
            object loadedValue = await ItemQueryHelper.SingleOrThrowAsync(selectScalarOnlyQuery, forEachAsync);

            if (value.Replacer != null)
            {
                await value.Replacer.LoadAsync(itemQuery);
                return value.Replacer.GetReplacement(loadedValue);
            }

            return loadedValue;
        }
    }
}