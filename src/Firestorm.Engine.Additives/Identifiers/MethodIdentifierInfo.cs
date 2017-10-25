using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Engine.Identifiers;

namespace Firestorm.Engine.Additives.Identifiers
{
    /// <summary>
    /// A method that can identify the item.
    /// </summary>
    public class MethodIdentifierInfo<TItem> : IIdentifierInfo<TItem>
    {
        private readonly MethodInfo _getterMethod;
        private readonly object _instance;

        public MethodIdentifierInfo(MethodInfo getterMethod, object instance)
        {
            _getterMethod = getterMethod;
            _instance = getterMethod.IsStatic ? null : instance;
        }

        public Type IdentifierType
        {
            get { return typeof(string); }
        }

        public Expression GetGetterExpression(ParameterExpression parameterExpr)
        {
            return null;
        }

        public Expression<Func<TItem, bool>> GetPredicate(string identifier)
        {
            return null;
        }

        public object GetValue(TItem item)
        {
            throw new NotImplementedException("Getting identifer for an exact item is not implemented.");
        }

        public void SetValue(TItem item, string identifier)
        {
            throw new NotImplementedException("Setting identifer value for an exact item is not implemented.");
        }

        public bool AllowsUpsert { get; }

        public TItem GetAlreadyLoadedItem(string identifier)
        {
            var parameters = _getterMethod.GetParameters();
            Debug.Assert(parameters.Length == 1, "Should be exactly 1 parameter by the time we get here.");

            Type paramType = parameters[0].ParameterType;
            object arg = ConversionUtility.ConvertString(identifier, paramType);

            object methodValue = _getterMethod.Invoke(_instance, new object[] { arg });

            if (methodValue is TItem)
                return (TItem)methodValue;

            throw new IdentifierMethodNullException();
        }
    }
}