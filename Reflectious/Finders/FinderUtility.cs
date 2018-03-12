using System;

namespace Firestorm
{
    internal static class FinderUtility
    {
        private static readonly Cache<CachedMethodFinder> MethodFinders = new Cache<CachedMethodFinder>();
        
        public static IMethodFinder GetMethodFinder(Type type, string methodName, object instance, Assume assume)
        {
            IMethodFinder finder =
            MethodFinders.GetOrAdd(type.GetHashCode() + methodName, 
                () => new CachedMethodFinder(GetNonCachedFinder(type, methodName, instance, assume)));

            return finder;
        }

        private static ICacheableMethodFinder GetNonCachedFinder(Type type, string methodName, object instance, Assume assume)
        {
            return assume.HasFlag(Assume.UnambiguousName)
                ? new SingleMethodFinder(type, methodName, instance == null)
                : (ICacheableMethodFinder) new MethodFinder(type, methodName, instance == null);
        }
    }
}