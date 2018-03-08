using System;
using System.Reflection;

namespace Firestorm
{
    public class PropertyFinder : MemberFinder, IPropertyFinder
    {
        public PropertyFinder(Type type, string propertyName, bool isStatic) 
            : base(type, propertyName, isStatic)
        {
        }
        
        public PropertyInfo Find()
        {
            PropertyInfo property = Type.GetProperty(MemberName, GetBindingFlags());

            if (property == null)
                throw new PropertyNotFoundException(MemberName);

            return property;
        }

        public Type PropertyType
        {
            get { return Find().PropertyType; }
        }
    }
}