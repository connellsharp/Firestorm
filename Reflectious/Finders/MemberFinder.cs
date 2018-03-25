using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Firestorm
{
    internal abstract class MemberFinder
    {
        protected Type ClassType { get; }
        protected string MemberName { get; }
        protected bool IsStatic { get; }

        internal MemberFinder(Type classType, string memberName, bool isStatic)
        {
            ClassType = classType;
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