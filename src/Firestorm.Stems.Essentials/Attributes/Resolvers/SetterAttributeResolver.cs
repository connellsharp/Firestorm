using System;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Stems.Analysis;

namespace Firestorm.Stems.Essentials.Resolvers
{
    internal class SetterAttributeResolver : FieldAttributeResolverBase
    {
        protected override void AddExpressionToDefinition(LambdaExpression expression)
        {
            FieldDefinition.Setter.Expression = expression;
        }

        protected override Type AddMethodToDefinition(MethodInfo method)
        {
            Type secondParamType = GetMethodSetterParamType(ItemType, method);
            Type delegateType = typeof(Action<,>).MakeGenericType(ItemType, secondParamType);

            AddMethodToHandlerPart(FieldDefinition.Setter, method, delegateType);

            return secondParamType;
        }

        private static Type GetMethodSetterParamType(Type itemType, MethodInfo method)
        {
            if (method.ReturnType != typeof(void))
                throw new StemAttributeSetupException("Setter methods must have a void return type.");

            ParameterInfo[] parameters = method.GetParameters();

            if (parameters.Length != 2)
                throw new StemAttributeSetupException("Setter methods must have two parameters for the item and field value.");

            if (parameters[0].ParameterType != itemType)
                throw new StemAttributeSetupException("A setter method's first parameter must be of type " + itemType.Name);

            return parameters[1].ParameterType;
        }
    }
}