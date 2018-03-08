using System;

namespace Firestorm
{
    public abstract class MemberNotFoundException : Exception
    {
        internal MemberNotFoundException(string message)
            : base(message)
        {
        }
    }
    
    public class MethodNotFoundException : MemberNotFoundException
    {
        public MethodNotFoundException(string methodName)
            : base("A method with the name '" + methodName + "' was not found.")
        {

        }
    }
    
    public class PropertyNotFoundException : MemberNotFoundException
    {
        public PropertyNotFoundException(string propertyName)
            : base("A property with the name '" + propertyName + "' was not found.")
        {

        }
    }
}