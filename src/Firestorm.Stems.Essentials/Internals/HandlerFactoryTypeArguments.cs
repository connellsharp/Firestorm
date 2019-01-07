using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Essentials.Factories.Resolvers;
using Reflectious;

namespace Firestorm.Stems.Essentials
{
    /// <summary>
    /// Contains the generic type arguments used to create handler factories in the <see cref="SubstemDefinitionResolver"/>.
    /// </summary>
    internal class HandlerFactoryTypeArguments
    {
        public void LoadExpression<TItem>(LambdaExpression getterExpression)
        {
            GetterExpression = getterExpression;

            PropertyValueType = ResolverTypeUtility.GetPropertyLambdaReturnType<TItem>(getterExpression.GetType());
            EnumerableTypeArgument = GetEnumerableTypeArgument(PropertyValueType);
        }

        public LambdaExpression GetterExpression { get; private set; }

        public Type PropertyValueType { get; private set; }

        public Type EnumerableTypeArgument { get; private set; }

        public bool IsCollection
        {
            get { return EnumerableTypeArgument != null; }
        }

        private static Type GetEnumerableTypeArgument(Type propertyValueType)
        {
            Type enumerableType = GetEnumerableType(propertyValueType);
            return enumerableType?.GetGenericArguments()[0];
        }

        private static Type GetEnumerableType(Type navPropertyType)
        {
            if (navPropertyType == typeof(string))
                return null;

            return navPropertyType.GetGenericInterface(typeof(IEnumerable<>));
        }
    }
}