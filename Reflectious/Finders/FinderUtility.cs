using System;

namespace Firestorm
{
    internal static class FinderUtility
    {
        private static readonly Cache<CachedMethodFinder> MethodFinders = new Cache<CachedMethodFinder>();
        
        public static IMethodFinder GetMethodFinder(Type classType, string methodName, bool isStatic, Assume assume)
        {
            IMethodFinder finder =
            MethodFinders.GetOrAdd(classType.GetHashCode() + methodName, 
                () => new CachedMethodFinder(GetNonCachedFinder(classType, methodName, isStatic, assume)));

            return finder;
        }

        private static ICacheableMethodFinder GetNonCachedFinder(Type classType, string methodName, bool isStatic, Assume assume)
        {
            if (assume.HasFlag(Assume.UnambiguousName))
                return new SingleMethodFinder(classType, methodName, isStatic);
            
            return new MethodFinder(classType, methodName, isStatic);
        }

        public static IMethodFinder WrapForExtension(IMethodFinder finder, Type extensionThisParamType)
        {       
            return new ExtensionMethodFinder(finder, extensionThisParamType);
        }
    }
}