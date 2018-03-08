using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Firestorm
{
    public class InstanceInvoker<TInstance> : InstanceInvoker
    {
        private readonly TInstance _instance;

        public InstanceInvoker([NotNull] TInstance instance)
            : base(instance)
        {
            _instance = instance;
        }

        internal InstanceInvoker(Type type)
            : base(type)
        {
        }

        public MethodInvoker<TInstance, TReturn> GetMethod<TReturn>(Expression<Func<TInstance, TReturn>> expression)
        {
            var finder = new ExpressionMethodFinder<TInstance, TReturn>(expression);
            return new MethodInvoker<TInstance, TReturn>(_instance, finder);
        }

        public PropertyInvoker<TInstance, TProperty> GetProperty<TProperty>(Expression<Func<TInstance, TProperty>> propertyExpression)
        {
            var finder = new ExpressionPropertyFinder<TInstance, TProperty>(propertyExpression);
            return new PropertyInvoker<TInstance, TProperty>(_instance, finder);
        }
    }

    public class InstanceInvoker
    {
        private readonly object _instance;
        private readonly Type _type;

        public InstanceInvoker([NotNull] object instance)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            _type = _instance.GetType();
        }

        internal InstanceInvoker(Type type)
        {
            _type = type;
        }

        public MethodInvoker GetMethod(string methodName)
        {
            var finder = new MethodFinder(_type, methodName, _instance == null);
            return new MethodInvoker(_instance, finder);
        }

        public PropertyInvoker GetProperty(string propertyName)
        {
            var finder = new PropertyFinder(_type, propertyName, _instance == null);
            return new PropertyInvoker(_instance, finder);
        }
    }
}