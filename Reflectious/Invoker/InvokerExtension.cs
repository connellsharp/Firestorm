using System;

namespace Firestorm
{
    public static class InvokerExtension
    {   
        public static InstanceInvoker<T> Invoker<T>(this T instance)
        {
            return new InstanceInvoker<T>(instance);
        }
        
        public static StaticInvoker Invoker(this Type type)
        {
            return new StaticInvoker(type);
        }
    }
}