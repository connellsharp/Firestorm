using System;
using System.Reflection;

namespace Firestorm
{
    internal class DelegateMethod : IMethod
    {
        private readonly Delegate _del;

        public DelegateMethod(Delegate del)
        {
            _del = del;
        }

        public object Invoke(object instance, object[] args)
        {
            object[] delegateArgs = GetDelegateArgs(instance, args);
            return _del.DynamicInvoke(delegateArgs);
        }

        private static object[] GetDelegateArgs(object instance, object[] args)
        {
            if (instance == null) 
                return args;
            
            object[] delegateArgs = new object[args.Length + 1];
            delegateArgs[0] = instance;
            args.CopyTo(delegateArgs, 1);
            return delegateArgs;
        }

        public MethodInfo GetMethodInfo()
        {
            return _del.Method;
        }
    }
}