using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm
{
    public class Invoker<TInstance> : Invoker
    {
        [NotNull] private readonly TInstance _instance;

        public Invoker([NotNull] TInstance instance)
            : base(instance)
        {
            _instance = instance;
        }

        public StrongReturnMethodInvoker<TInstance, TReturn> GetMethod<TReturn>(Expression<Func<TInstance, TReturn>> expression)
        {
            return new StrongReturnMethodInvoker<TInstance, TReturn>(_instance, expression);
        }
    }

    public class Invoker
    {
        private readonly object _instance;
        private readonly Type _type;

        public Invoker([NotNull] object instance)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            _type = _instance.GetType();
        }

        public MethodInvoker GetMethod(string methodName)
        {
            MethodInfo method = _type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            return new MethodInvoker(_instance, method);
        }
    }
}