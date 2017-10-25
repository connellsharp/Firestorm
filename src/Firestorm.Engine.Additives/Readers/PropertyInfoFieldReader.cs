using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives.Readers
{
    /// <summary>
    /// Uses <see cref="PropertyInfo"/> of a property to read from a field using reflection.
    /// </summary>
    public class PropertyInfoFieldReader<TItem> : BasicFieldReaderBase<TItem>
        where TItem : class
    {
        private readonly PropertyInfo _propertyInfo;

        public PropertyInfoFieldReader([NotNull] PropertyInfo propertyInfo)
        {
            PropertyInfoUtilities.EnsureValidProperty<TItem>(propertyInfo);
            _propertyInfo = propertyInfo;
        }

        protected override Expression GetGetterExpression(ParameterExpression parameterExpr)
        {
            return Expression.Property(parameterExpr, _propertyInfo);
        }

        public override Type FieldType
        {
            get { return _propertyInfo.PropertyType; }
        }
    }
}