using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm.Engine.Additives.Writers
{
    /// <summary>
    /// Uses the <see cref="PropertyInfo"/> from the property <see cref="Expression"/> to write to a field.
    /// </summary>
    public class PropertyExpressionFieldWriter<TItem, TProperty> : PropertyInfoFieldWriter<TItem>
        where TItem : class
    {
        public PropertyExpressionFieldWriter([NotNull] Expression<Func<TItem, TProperty>> propertyExpression)
            : base(PropertyInfoUtilities.GetPropertyInfoFromLambda(propertyExpression))
        { }
    }
}