using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm.Stems.AutoMap
{
    /// <summary>
    /// Automatically finds Expressions for Stem properties that do not return Expressions.
    /// </summary>
    public interface IPropertyAutoMapper
    {
        [NotNull]
        LambdaExpression MapExpression(PropertyInfo property, Type itemType);
    }
}