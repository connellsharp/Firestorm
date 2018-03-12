using System;

namespace Firestorm
{
    public static class ReflectorExtension
    {   
        public static InstanceReflector<T> Reflect<T>(this T instance)
        {
            return new InstanceReflector<T>(instance);
        }
        
        public static StaticReflector Reflect(this Type type)
        {
            return new StaticReflector(type);
        }
    }
}