using System;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Engine.Identifiers;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives.Identifiers
{
    /// <summary>
    /// An identifier based on <see cref="PropertyInfo"/>.
    /// </summary>
    public class PropertyInfoIdentifierInfo<TItem> : IIdentifierInfo<TItem>
    {
        private readonly PropertyInfo _propertyInfo;

        public PropertyInfoIdentifierInfo([NotNull] PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            if(!typeof(TItem).IsAssignableFrom(propertyInfo.DeclaringType))
                throw new ArgumentException("Generic type parameter must be a sub class off the given property info declaring type.");

            _propertyInfo = propertyInfo;
        }

        public Type IdentifierType
        {
            get { return _propertyInfo.PropertyType; }
        }

        public object GetValue(TItem item)
        {
            return _propertyInfo.GetValue(item);
        }

        public Expression GetGetterExpression(ParameterExpression parameterExpr)
        {
            return Expression.Property(parameterExpr, _propertyInfo);
        }

        public Expression<Func<TItem, bool>> GetPredicate(string identifier)
        {
            object identifierObj = ConversionUtility.ConvertString(identifier, _propertyInfo.PropertyType);
            ParameterExpression param = Expression.Parameter(typeof(TItem), "t");
            Expression propertyExpr = Expression.Property(param, _propertyInfo);
            BinaryExpression predicateExpr = Expression.Equal(propertyExpr, Expression.Constant(identifierObj, _propertyInfo.PropertyType));
            return Expression.Lambda<Func<TItem, bool>>(predicateExpr, param);
        }

        public void SetValue(TItem item, string identifier)
        {
            var newReference = ConversionUtility.ConvertString(identifier, _propertyInfo.PropertyType);
            _propertyInfo.SetValue(item, newReference);
        }

        public bool AllowsUpsert { get; } = true;

        public TItem GetAlreadyLoadedItem(string identifier)
        {
            return default(TItem);
        }
    }
}