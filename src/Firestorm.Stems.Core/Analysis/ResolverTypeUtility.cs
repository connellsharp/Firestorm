using System;
using Reflectious;

namespace Firestorm.Stems.Analysis
{
    public static class ResolverTypeUtility
    {
        public static Type GetPropertyLambdaReturnType<TItem>(Type expressionPropertyType)
        {
            return GetPropertyLambdaReturnType(typeof(TItem), expressionPropertyType);
        }

        public static Type GetPropertyLambdaReturnType(Type itemType, Type expressionPropertyType)
        {
            Type exprType = expressionPropertyType.GenericTypeArguments[0];

            return GetLambdaReturnType(itemType, exprType);
        }

        private static Type GetLambdaReturnType(Type itemType, Type funcType)
        {
            if (!funcType.IsSubclassOfGeneric(typeof(Func<,>)))
                throw new StemAttributeSetupException("Expression types must be lambda expressions of type Expression<Func<,>>.");

            if (funcType.GenericTypeArguments[0] != itemType)
                throw new StemAttributeSetupException("Lambda expression first generic argument must be of type " + itemType.Name);

            return funcType.GenericTypeArguments[1];
        }
    }
}