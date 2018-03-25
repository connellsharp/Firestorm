using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Firestorm
{
    internal class ConstructorFinder : IMethodFinder
    {
        protected readonly Type Type;

        public ConstructorFinder(Type type)
        {
            Type = type;
        }

        public Type[] GenericArguments { get; set; }
        public Type[] ParameterTypes { get; set; }
        public bool WantsParameterTypes => ParameterTypes == null;

        public string GetCacheKey()
        {
            var builder = new StringBuilder(".ctor");
            builder.AppendFullTypeNames(GenericArguments);
            builder.AppendFullTypeNames(ParameterTypes);
            return builder.ToString();
        }

        public IMethod Find()
        {
            Type type = GetGenericType();
            
            return new ActivatorConstructor(type);
            
            var ctorInfo = FindConstructorInfo(type);
            return new ReflectionConstructor(ctorInfo);
        }

        private ConstructorInfo FindConstructorInfo(Type type)
        {
            IEnumerable<ConstructorInfo> ctors = type.GetConstructors();

            if (ParameterTypes != null)
                ctors = ctors.Where(c => MatchUtilities.MatchesParameterTypes(c, ParameterTypes));

            var ctorsList = ctors.ToList();

            switch (ctorsList.Count)
            {
                case 0:
                    throw new MethodNotFoundException(".ctor");

                case 1:
                    return ctorsList[0];

                default:
                    var ctor = ctorsList.Find(c => c.GetParameters().Length == 0);
                    return ctor ?? throw new MethodNotFoundException(".ctor");
            }
        }

        private Type GetGenericType()
        {
            Type type = Type;

            if (GenericArguments != null)
                type = type.MakeGenericType(GenericArguments);
            
            return type;
        }
    }
}