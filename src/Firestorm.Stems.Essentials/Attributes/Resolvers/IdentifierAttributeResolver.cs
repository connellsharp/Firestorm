using System;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Definitions;
using Reflectious;

namespace Firestorm.Stems.Essentials.Resolvers
{
    internal class IdentifierAttributeResolver : AttributeResolverBase
    {
        private readonly bool _isMultiReference;

        public IdentifierAttributeResolver(bool isMultiReference)
        {
            _isMultiReference = isMultiReference;
        }

        private IdentifierDefinition IdentifierDefinition { get; set; }

        private void SetIdentifierDefinition(MemberInfo member)
        {
            if (!(Attribute is IdentifierAttribute identifierAttribute))
                throw new StemAttributeSetupException("Attribute used a resolver for a FieldAttribute, but was not a FieldAttribute.");

            string name = identifierAttribute.Name ?? identifierAttribute.GetDefaultName(member.Name);
            IdentifierDefinition = Definition.IdentifierDefinitions.GetOrCreate(name);

            IdentifierDefinition.ExactValue = identifierAttribute.Exactly;
        }

        public override void IncludeMember(MemberInfo member)
        {
            SetIdentifierDefinition(member);
            base.IncludeMember(member);
        }

        protected override void IncludeMethod(MethodInfo method)
        {
            if (method.ReturnType == typeof(void))
            {
                AddSetterMethod(method);
            }
            else if (method.ReturnType.IsSubclassOfGeneric(typeof(Expression)))
            {
                IdentifierDefinition.PredicateMethod = method;
            }
            else if (method.ReturnType == ItemType)
            {
                switch (method.GetParameters().Length)
                {
                    case 0:
                        IdentifierDefinition.ExactGetterMethod = method;
                        break;

                    case 1:
                        IdentifierDefinition.GetterMethod = method;
                        break;

                    default:
                        throw new StemAttributeSetupException("Identifier attribute placed on a method that returns an item but has too many parameters.");
                }
            }
            else
                throw new StemAttributeSetupException("Identifier attribute placed on a method with an invalid signature.");
        }

        protected override void IncludeExpression(LambdaExpression expression)
        {
            IdentifierDefinition.GetterExpression = expression;
            IdentifierDefinition.IsMultiReference = _isMultiReference;
        }

        protected virtual void AddSetterMethod(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length != 2 || parameters[0].ParameterType != ItemType || parameters[1].ParameterType != typeof(string))
                throw new StemAttributeSetupException("Identifier attribute for setters must be placed on void methods with two parameters for the item and string identifier.");

            Type actionType = typeof(Action<,>).MakeGenericType(ItemType, typeof(string));
            IdentifierDefinition.SetterAction = method.CreateDelegate(actionType);
        }
    }
}