using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Firestorm
{
    internal abstract class MemberFinder
    {
        protected Type Type { get; }
        protected string MemberName { get; }
        protected bool IsStatic { get; }

        internal MemberFinder(Type type, string memberName, bool isStatic)
        {
            Type = type;
            MemberName = memberName;
            IsStatic = isStatic;
        }

        private const BindingFlags Flags = BindingFlags.Public |
                                           BindingFlags.NonPublic |
                                           BindingFlags.Static |
                                           BindingFlags.Instance;

        protected BindingFlags GetBindingFlags()
        {
            if (IsStatic)
                return BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            else
                return BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        }
    }
}