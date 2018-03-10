using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Firestorm
{
    public class ConstructorFinder : IMethodFinder
    {
        protected readonly Type Type;

        public ConstructorFinder(Type type)
        {
            Type = type;
        }

        public Type[] GenericArguments { get; set; }
        public Type[] ParameterTypes { get; set; }

        public ConstructorInfo FindConstructorInfo()
        {
            Type type = Type;

            if (GenericArguments != null)
                type = type.MakeGenericType(GenericArguments);
            
            IEnumerable<ConstructorInfo> ctors = type.GetConstructors();

            if (ParameterTypes != null)
                ctors = ctors.Where(m => m.GetParameters().Length == ParameterTypes.Length); // TODO check types?

            var ctorsList = ctors.ToList();
            
            if (ctorsList.Count == 0)
                throw new MethodNotFoundException(".ctor");

            ConstructorInfo ctor = ctorsList.Single();

            return ctor;
        }

        public MethodInfo FindMethodInfo()
        {
            throw new InvalidOperationException("Cannot get the MethodInfo for a constructor.");
        }

        public object FindAndInvoke(object instance, object[] args)
        {
            if (instance != null)
                throw new InvalidOperationException("A constructor cannot be called on an object that has already been instantiated.");
            
            ConstructorInfo ctor = FindConstructorInfo();
            return ctor.Invoke(args);
        }
    }
}