using System;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Stems.Attributes.Analysis;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel.Identifiers
{
    /// <summary>
    /// Builds identifier info from <see cref="IdentifierAttribute"/>s placed on properties and predicate methods in a Stem.
    /// </summary>
    internal class PredicateMethodIdentifierInfo<TItem> : PredicateIdentifierInfo<TItem>
        where TItem : class
    {
        public PredicateMethodIdentifierInfo(MethodInfo getPredicateMethod, [CanBeNull] Action<TItem, string> setter)
            : base(GetPredicateFunc(getPredicateMethod), setter)
        {
        }

        private static Func<string, Expression<Func<TItem, bool>>> GetPredicateFunc(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length != 1 || parameters[0].ParameterType != typeof(string))
                throw new StemAttributeSetupException("Identifier attribute must be placed on methods with a single string parameter.");

            Type returnType = typeof(Expression<Func<TItem, bool>>);
            if (method.ReturnType != returnType)
                throw new StemAttributeSetupException("Identifier attribute must be placed on methods with a predicate return stemType of " + returnType.Name);

            var getPredicateFunc = (Func<string, Expression<Func<TItem, bool>>>)method.CreateDelegate(typeof(Func<string, Expression<Func<TItem, bool>>>));
            return getPredicateFunc;
        }
    }
}