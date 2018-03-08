using System;
using System.Reflection;

namespace Firestorm
{
    public abstract class MemberFinder
    {
        protected Type Type { get; }
        protected string MemberName { get; }
        
        private readonly bool _isStatic;

        internal MemberFinder(Type type, string memberName, bool isStatic)
        {
            Type = type;
            MemberName = memberName;
            _isStatic = isStatic;
        }

        private const BindingFlags Flags = BindingFlags.Public |
                                           BindingFlags.NonPublic |
                                           BindingFlags.Static |
                                           BindingFlags.Instance;

        protected BindingFlags GetBindingFlags()
        {
            if (_isStatic)
                return BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            else
                return BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        }
    }
}