using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Firestorm
{
    public class InstanceReflector<TInstance> : InstanceReflector
    {
        private readonly TInstance _instance;

        public InstanceReflector([NotNull] TInstance instance)
            : base(instance)
        {
            _instance = instance;
        }

        internal InstanceReflector(Type type)
            : base(type)
        {
        }

        public MethodReflector<TInstance, TReturn> GetMethod<TReturn>(Expression<Func<TInstance, TReturn>> expression)
        {
            var finder = new ExpressionMethodFinder<TInstance, TReturn>(expression);
            return new MethodReflector<TInstance, TReturn>(_instance, finder);
        }

        public PropertyReflector<TInstance, TProperty> GetProperty<TProperty>(Expression<Func<TInstance, TProperty>> propertyExpression)
        {
            var finder = new ExpressionPropertyFinder<TInstance, TProperty>(propertyExpression);
            return new PropertyReflector<TInstance, TProperty>(_instance, finder);
        }
    }

    public class InstanceReflector
    {
        private readonly object _instance;
        protected readonly Type Type;

        public InstanceReflector([NotNull] object instance)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Type = _instance.GetType();
        }

        internal InstanceReflector(Type type)
        {
            Type = type;
        }

        public MethodReflector GetMethod(string methodName, Assume assume = Assume.Nothing)
        {
            var finder = FinderUtility.GetMethodFinder(Type, methodName, _instance, assume);
            
            return new MethodReflector(_instance, finder);
        }

        public PropertyReflector GetProperty(string propertyName)
        {
            var finder = new PropertyFinder(Type, propertyName, _instance == null);
            return new PropertyReflector(_instance, finder);
        }
    }
}