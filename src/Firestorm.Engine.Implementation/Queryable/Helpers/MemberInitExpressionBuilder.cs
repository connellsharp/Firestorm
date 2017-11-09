using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine.Queryable.Helpers
{
    /// <summary>
    /// Builds <see cref="MemberInitExpression"/> objects for a dynamic type.
    /// </summary>
    public class MemberInitExpressionBuilder
    {
        private readonly Type _dynamicType;

        /// <summary>
        /// Builds <see cref="MemberInitExpression"/> objects for the given <see cref="dynamicType"/>
        /// </summary>
        public MemberInitExpressionBuilder(Type dynamicType)
        {
            _dynamicType = dynamicType;
        }

        [Pure]
        public MemberInitExpression Build<TItem>(ParameterExpression itemPram, IDictionary<string, IFieldReader<TItem>> fieldReaders)
            where TItem : class
        {
            return Build(delegate (FieldInfo fi)
            {
                Debug.Assert(fieldReaders.ContainsKey(fi.Name));
                return Expression.Bind(fi, fieldReaders[fi.Name].GetSelectExpression(itemPram));
            });
        }

        [Pure]
        public MemberInitExpression Build<TItem>(ParameterExpression itemPram, IFieldProvider<TItem> fieldProvider)
            where TItem : class
        {
            return Build(delegate (FieldInfo fi)
            {
                IFieldReader<TItem> reader = fieldProvider.GetReader(fi.Name);
                Debug.Assert(reader != null, "Reader should definitely not be null here.");
                return Expression.Bind(fi, reader.GetSelectExpression(itemPram));
            });
        }

        private MemberInitExpression Build(Func<FieldInfo, MemberAssignment> selector)
        {
            IEnumerable<MemberBinding> bindings = _dynamicType.GetFields().Select(selector);
            NewExpression newExpr = Expression.New(_dynamicType);
            return Expression.MemberInit(newExpr, bindings);
        }
    }
}