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
        protected readonly Type Type;

        public InstanceInvoker([NotNull] object instance)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Type = _instance.GetType();
        }

        internal InstanceInvoker(Type type)
        {
            Type = type;
        }

        public MethodInvoker GetMethod(string methodName, Assume assume = Assume.Nothing)
        {
            var finder = assume.HasFlag(Assume.UnambiguousName)
                ? new SingleMethodFinder(Type, methodName, _instance == null)
                : (IMethodFinder) new MethodFinder(Type, methodName, _instance == null);
            
            return new MethodInvoker(_instance, finder);
        }

        public PropertyInvoker GetProperty(string propertyName)
        {
            var finder = new PropertyFinder(Type, propertyName, _instance == null);
            return new PropertyInvoker(_instance, finder);
        }
    }
}