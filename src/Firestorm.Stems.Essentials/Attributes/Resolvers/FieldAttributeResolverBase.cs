using System;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Definitions;

namespace Firestorm.Stems.Essentials.Resolvers
{
    /// <summary>
    /// Reflection methods for building <see cref="FieldDefinition" /> objects from reading <see cref="FieldAttribute" />s on a type.
    /// </summary>
    internal abstract class FieldAttributeResolverBase : AttributeResolverBase
    {
        protected FieldDefinition FieldDefinition { get; private set; }

        protected void SetFieldDefinition(MemberInfo member)
        {
            if (!(Attribute is FieldAttribute fieldAttribute))
                throw new StemAttributeSetupException("Attribute used a resolver for a FieldAttribute, but was not a FieldAttribute.");
            
            string fieldName = fieldAttribute.Name ?? fieldAttribute.GetDefaultName(member.Name);
            FieldDefinition = Definition.FieldDefinitions.GetOrCreate(fieldName);
        }

        public override void IncludeMember(MemberInfo member)
        {
            SetFieldDefinition(member);
            IncludeMemberToDefinition(member);
        }

        protected void IncludeMemberToDefinition(MemberInfo member)
        {
            base.IncludeMember(member);
        }

        protected override void IncludeExpression(LambdaExpression expression)
        {
            Type exprBodyType = ResolverTypeUtility.GetPropertyLambdaReturnType(ItemType, expression.GetType());
            SetOrConfirmType(exprBodyType);

            AddExpressionToDefinition(expression);
        }

        protected sealed override void IncludeMethod(MethodInfo method)
        {
            Type fieldType = AddMethodToDefinition(method);

            SetOrConfirmType(fieldType);
        }

        protected abstract void AddExpressionToDefinition(LambdaExpression expression);

        protected abstract Type AddMethodToDefinition(MethodInfo method);

        protected void SetOrConfirmType(Type fieldType)
        {
            if (FieldDefinition.FieldType == null)
            {
                FieldDefinition.FieldType = fieldType;
            }
            else if (FieldDefinition.FieldType != fieldType)
            {
                throw new StemAttributeSetupException(string.Format("The '{0}' field appears to be multiple different types: {1} and {2}.", FieldDefinition.FieldName,
                    FieldDefinition.FieldType, fieldType));
            }
        }

        protected static void AddMethodToHandlerPart(FieldDefinitionHandlerPart handlerPart, MethodInfo method, Type delegateType)
        {
            if (method.IsStatic)
            {
                Delegate staticMethod = Delegate.CreateDelegate(delegateType, method);
                handlerPart.GetInstanceMethod = stemInstance => staticMethod;
            }
            else
            {
                handlerPart.GetInstanceMethod = stemInstance => Delegate.CreateDelegate(delegateType, stemInstance, method);
            }
        }
    }
}