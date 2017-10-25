using System;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Engine.Identifiers;

namespace Firestorm.Engine.Additives.Identifiers
{
    /// <summary>
    /// An exact identifier, designed to return an exact item when an exact identifier string is used.
    /// Common example would be /users/me
    /// </summary>
    public class ExactIdentifierInfo<TItem> : IIdentifierInfo<TItem>
        where TItem : class
    {
        private readonly string _exactValue;
        private readonly object _instance;
        private readonly MethodInfo _exactGetterMethod;

        public ExactIdentifierInfo(string exactValue, object instance, MethodInfo exactGetterMethod)
        {
            _exactValue = exactValue;
            _instance = exactGetterMethod.IsStatic ? null : instance;
            _exactGetterMethod = exactGetterMethod;
        }

        public Type IdentifierType
        {
            get { return _exactGetterMethod.ReturnType; }
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
            if (identifier == _exactValue)
            {
                object methodValue = _exactGetterMethod.Invoke(_instance, new object[] { });

                if (methodValue is TItem)
                    return (TItem) methodValue;

                throw new IdentifierMethodNullException();
            }

            return null;
        }
    }
}