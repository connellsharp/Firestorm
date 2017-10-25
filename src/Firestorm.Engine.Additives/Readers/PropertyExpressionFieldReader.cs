using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives.Readers
{
    /// <summary>
    /// Uses the <see cref="PropertyInfo"/> from the property <see cref="Expression"/> to read from a field.
    /// </summary>
    public class PropertyExpressionFieldReader<TItem, TProperty> : PropertyInfoFieldReader<TItem>
        where TItem : class
    {
        public PropertyExpressionFieldReader([NotNull] Expression<Func<TItem, TProperty>> propertyExpression)
            : base(PropertyInfoUtilities.GetPropertyInfoFromLambda(propertyExpression))
        { }
    }
}