using System;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Stems.AutoMap;
using Firestorm.Stems.Definitions;
using JetBrains.Annotations;

namespace Firestorm.Stems.Analysis
{
    /// <summary>
    /// Base class for most <see cref="IAttributeResolver"/>s.
    /// Can do basic splitting of member types and provides methods to override.
    /// </summary>
    public class AttributeResolverBase : IAttributeResolver
    {
        public Type ItemType { protected get; set; }

        public StemDefinition Definition { protected get; set; }

        public IPropertyAutoMapper PropertyAutoMapper { protected get; set; }

        public StemAttribute Attribute { protected get; set; }

        public virtual void IncludeMember(MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Method)
            {
                IncludeMethod((MethodInfo) member);
            }
            else if (member.MemberType == MemberTypes.Property)
            {
                IncludeProperty((PropertyInfo) member);
            }
        }

        protected virtual void IncludeMethod(MethodInfo method)
        {
            throw new StemAttributeSetupException("Attribute has been applied to a method, which cannot be resolved.");
        }

        protected virtual void IncludeProperty(PropertyInfo property)
        {
            if(!property.GetGetMethod().IsStatic)
                throw new StemAttributeSetupException("Stem properties must be static. Try using a method instead.");
            
            LambdaExpression expression = GetExpressionFromProperty(property);
            IncludeExpression(expression);
        }

        protected virtual LambdaExpression GetExpressionFromProperty(PropertyInfo property)
        {
            if (typeof(Expression).IsAssignableFrom(property.PropertyType))
                return GetPropertyReturnExpression(property);
            
            if(property.GetCustomAttribute<AutoExprAttribute>() != null)
                return GetAutoMapExpression(property);
            
            throw new StemAttributeSetupException("Attribute has been applied to a property that cannot be resolved.");
        }

        private LambdaExpression GetPropertyReturnExpression(PropertyInfo property)
        {
            var expression = property.GetValue(null);

            if (expression == null)
                throw new StemAttributeSetupException("Returned expression from property was null.");

            if (!(expression is LambdaExpression lambdaExpression))
                throw new StemAttributeSetupException("Returned expression from property was not a LambdaExpression.");
            
            return lambdaExpression;
        }

        protected virtual void IncludeExpression([NotNull] LambdaExpression expression)
        {
            throw new StemAttributeSetupException("Attribute has been applied to an Expression property, which cannot be resolved.");
        }

        private LambdaExpression GetAutoMapExpression(PropertyInfo property)
        {
            if (PropertyAutoMapper == null)
                throw new StemAttributeSetupException("Attribute has been applied to an auto expression property, but no AutoPropertyMapper is set up.");

            LambdaExpression expression;

            try
            {
                expression = PropertyAutoMapper.MapExpression(property, ItemType);
            }
            catch (Exception ex)
            {
                throw new StemAttributeSetupException("Error resolving expression from auto property. See inner exception for details.", ex);
            }

            if (expression == null)
                throw new StemAttributeSetupException("Returned lambda expression from auto mapper was null.");

            return expression;
        }
    }
}