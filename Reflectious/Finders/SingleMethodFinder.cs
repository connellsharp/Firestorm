using System;
using System.Reflection;
using System.Text;

namespace Firestorm
{
    internal class SingleMethodFinder : MemberFinder, ICacheableMethodFinder
    {
        public SingleMethodFinder(Type classType, string methodName, bool isStatic) 
            : base(classType, methodName, isStatic)
        {
        }

        public Type[] GenericArguments { get; set; }
        public Type[] ParameterTypes { get; set; }
        public bool WantsParameterTypes { get; } = false;

        public string GetCacheKey()
        {
            var builder = new StringBuilder(MemberName);
            builder.AppendFullTypeNames(GenericArguments);
            builder.AppendFullTypeNames(ParameterTypes);
            return builder.ToString();
        }

        public IMethod Find()
        {
            MethodInfo method = ClassType.GetMethod(MemberName, GetBindingFlags());
            return new ReflectionMethod(method);
        }
    }
}