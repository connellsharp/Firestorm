using System;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.AutoMap;
using JetBrains.Annotations;

namespace Firestorm.Stems.Attributes.Analysis
{
    /// <summary>
    /// Base class for most <see cref="IAttributeResolver"/>s.
    /// Can do basic splitting of member types and provides methods to override.
    /// </summary>
    public class AttributeResolverBase : IAttributeResolver
    {
        public Type ItemType { protected get; set; }

        public StemDefinition Definition { protected get; set; }

        public IStemConfiguration Configuration { protected get; set; }

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
            if (property.PropertyType.IsSubclassOfGeneric(typeof(Expression<>)))
                return GetPropertyReturnExpression(property);
            else
                return GetAutoMapExpression(property);
        }

        private LambdaExpression GetPropertyReturnExpression(PropertyInfo property)
        {
            var exprBodyType = ResolverTypeUtility.GetPropertyLambdaReturnType(ItemType, property.PropertyType);

            var expression = property.GetValue(null) as LambdaExpression;

            if (expression == null)
                throw new StemAttributeSetupException("Returned lambda expression from property was null.");

            return expression;
        }

        protected virtual void IncludeExpression([NotNull] LambdaExpression expression)
        {
            throw new StemAttributeSetupException("Attribute has been applied to an Expression property, which cannot be resolved.");
        }

        private LambdaExpression GetAutoMapExpression(PropertyInfo property)
        {
            IPropertyAutoMapper autoPropertyMapper = Configuration?.AutoPropertyMapper;
            if (autoPropertyMapper == null)
                throw new StemAttributeSetupException("Attribute has been applied to a mappable property, which cannot be resolved.");

            LambdaExpression expression;

            try
            {
                expression = autoPropertyMapper.MapExpression(property, ItemType);
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