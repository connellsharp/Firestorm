using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Identifiers;
using Firestorm.Stems.Attributes;
using Firestorm.Stems.Attributes.Analysis;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel.Identifiers
{
    /// <summary>
    /// Builds identifier info from <see cref="IdentifierAttribute"/>s placed on properties, predicate methods and setter methods in a Stem.
    /// </summary>
    public class ExpressionAndSetterIdentifierInfo<TItem> : IIdentifierInfo<TItem>
        where TItem : class
    {
        private readonly IIdentifierInfo<TItem> _getterIdentifierInfo;
        private readonly Action<TItem, string> _setter;

        public ExpressionAndSetterIdentifierInfo([NotNull] LambdaExpression getterExpr, [CanBeNull] Action<TItem, string> setter, bool multiReferenceCollection)
        {
            _setter = setter;

            _getterIdentifierInfo = GetGetterIdentifierInfo(getterExpr, multiReferenceCollection);
        }

        private IIdentifierInfo<TItem> GetGetterIdentifierInfo(LambdaExpression getterExpr, bool multiReferenceCollection)
        {
            Type returnType = ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(getterExpr.GetType());

            if (multiReferenceCollection)
            {
                Type enumerableType = returnType.GetGenericInterface(typeof(IEnumerable<>));
                if (enumerableType == null)
                    throw new StemAttributeSetupException("References attribute must be placed on expressions that return an IEnumerable.");

                Type itemType = enumerableType.GetGenericArguments()[0];

                var type = typeof(MultiExpressionIdentifierInfo<,>).MakeGenericType(typeof(TItem), itemType);
                return (IIdentifierInfo<TItem>) Activator.CreateInstance(type, getterExpr);
            }
            else
            {
                var type = typeof(ExpressionIdentifierInfo<,>).MakeGenericType(typeof(TItem), returnType);
                return (IIdentifierInfo<TItem>) Activator.CreateInstance(type, getterExpr);
            }
        }

        public Type IdentifierType
        {
            get { return _getterIdentifierInfo.IdentifierType; }
        }

        public object GetValue(TItem item)
        {
            return _getterIdentifierInfo.GetValue(item);
        }

        public Expression GetGetterExpression(ParameterExpression parameterExpr)
        {
            return _getterIdentifierInfo.GetGetterExpression(parameterExpr);
        }

        public Expression<Func<TItem, bool>> GetPredicate(string identifier)
        {
            return _getterIdentifierInfo.GetPredicate(identifier);
        }

        public void SetValue(TItem item, string identifier)
        {
            if (_setter != null)
            {
                _setter.Invoke(item, identifier);
                return;
            }

            if (_getterIdentifierInfo.AllowsUpsert)
            {
                _getterIdentifierInfo.SetValue(item, identifier);
                return;
            }

            throw new NotSupportedException("Upsert by setting the identifier is not supported in this collection.");
        }

        public bool AllowsUpsert
        {
            get { return _setter != null || _getterIdentifierInfo.AllowsUpsert; }
        }

        public TItem GetAlreadyLoadedItem(string identifier)
        {
            return null;
        }
    }
}