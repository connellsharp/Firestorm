using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Firestorm
{
    public static class InvokerExtension
    {   
        public static Invoker<T> Invoker<T>(this T instance)
        {
            return new Invoker<T>(instance);
        }
    }
    
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

    public class StrongReturnMethodInvoker<TInstance, TReturn> : MethodInvoker
    {
        [NotNull] private readonly Expression<Func<TInstance, TReturn>> _expression;

        public StrongReturnMethodInvoker(TInstance instance, [NotNull] Expression<Func<TInstance, TReturn>> expression)
            : base(instance, LambdaMemberUtilities.GetMethodInfoFromLambda(expression))
        {
            _expression = expression;
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

    public class MethodInvoker
    {
        private readonly object _instance;
        private readonly MethodInfo _method;

        public MethodInvoker(object instance, [NotNull] MethodInfo method)
        {
            _instance = instance;
            _method = method ?? throw new ArgumentNullException(nameof(method));
        }

        public MethodInvoker MakeGeneric<T>()
        {
            return MakeGeneric(typeof(T));
        }

        public MethodInvoker MakeGeneric<T1, T2>()
        {
            return MakeGeneric(typeof(T1), typeof(T2));
        }

        public MethodInvoker MakeGeneric<T1, T2, T3>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3));
        }

        public MethodInvoker MakeGeneric<T1, T2, T3, T4>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        public MethodInvoker MakeGeneric<T1, T2, T3, T4, T5>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        public MethodInvoker MakeGeneric<T1, T2, T3, T4, T5, T6>()
        {
            return MakeGeneric(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
        }

        public MethodInvoker MakeGeneric(params Type[] types)
        {
            var genericMethod = _method.MakeGenericMethod(types);
            return new MethodInvoker(_instance, genericMethod);
        }
        
        public MethodInvoker MakeGeneric(IEnumerable<Type> types)
        {
            return MakeGeneric(types.ToArray());
        }
        
        public object Invoke(params object[] args)
        {
            return _method.Invoke(_instance, args);
        }
    }

    public class GenericMethodInvoker
    {
        public void Invoke(params object[] args)
        {
            
        }
    }
}