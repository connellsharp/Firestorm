using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

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
        /// Loads a single scalar field reader from a single <see cref="itemQuery"/>.
        /// </summary>
        public static async Task<object> LoadScalarValueAsync<TItem>([NotNull] IFieldReader<TItem> reader, IQueryableSingle<TItem> itemQuery, ForEachAsyncDelegate<object> forEachAsync)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            ParameterExpression itemPram = Expression.Parameter(typeof(TItem), "itm");
            Expression selectExpression = reader.GetSelectExpression(itemPram);
            
            IQueryable selectScalarOnlyQuery = ExpressionTreeHelpers.GetSelectExpressionQuery(itemQuery, itemPram, selectExpression);
            object loadedValue = await ItemQueryHelper.SingleOrCreateAsync(selectScalarOnlyQuery, forEachAsync, () => throw new ParentItemNotFoundException());

            if (reader.Replacer != null)
            {
                await reader.Replacer.LoadAsync(itemQuery);
                return reader.Replacer.GetReplacement(loadedValue);
            }

            return loadedValue;
        }

        private class ParentItemNotFoundException : RestApiException
        {
            public ParentItemNotFoundException()
                : base(ErrorStatus.NotFound, "The item was not found.")
            { }
        }
    }
}