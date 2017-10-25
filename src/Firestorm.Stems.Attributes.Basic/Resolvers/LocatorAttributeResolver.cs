using System;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Stems.Attributes.Analysis;
using Firestorm.Stems.Attributes.Definitions;

namespace Firestorm.Stems.Basic.Resolvers
{
    internal class LocatorAttributeResolver : FieldAttributeResolverBase
    {
        protected override void AddExpressionToDefinition(LambdaExpression expression)
        {
            FieldDefinition.Locator.Expression = expression;
        }

        protected override Type AddMethodToDefinition( MethodInfo method)
        {
            Tuple<Type, Type> paramTypes = GetMethodLocatorParamTypes(ItemType, method);
            Type delegateType = typeof(Func<,,>).MakeGenericType(paramTypes.Item1, paramTypes.Item2, ItemType);
            
            AddMethodToHandlerPart(FieldDefinition.Locator, method, delegateType);

            return paramTypes.Item1;
        }

        private static Tuple<Type, Type> GetMethodLocatorParamTypes(Type itemType, MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();

            if (method.ReturnType != itemType)
                throw new StemAttributeSetupException("A locator method's return type must be of type " + itemType.Name);

            if (parameters.Length != 2)
                throw new StemAttributeSetupException("Locator methods must have two parameters for the field value and the parent.");

            return new Tuple<Type, Type>(parameters[0].ParameterType, parameters[1].ParameterType);
        }
    }
}