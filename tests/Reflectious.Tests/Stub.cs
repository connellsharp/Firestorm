using System;

namespace Firestorm.Tests
{
    internal class Stub
    {
        internal const string MethodExecutedString = "Method executed";
        internal const string InitialPropertyValue = "Property value";
        
        public static string StaticProperty { get; set; } = InitialPropertyValue;

        public static string DoStaticMethod()
        {
            return MethodExecutedString;
        }
        
        public string InstanceProperty { get; set; } = InitialPropertyValue;

        public string DoInstanceMethod()
        {
            return MethodExecutedString;
        }
    }
}