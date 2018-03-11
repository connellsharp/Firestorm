using System;
using System.Reflection;

namespace Firestorm
{
    internal class PropertyFinder : MemberFinder, IPropertyFinder
    {
        public PropertyFinder(Type type, string propertyName, bool isStatic) 
            : base(type, propertyName, isStatic)
        {
        }
        
        public IProperty Find()
        {
            PropertyInfo property = Type.GetProperty(MemberName, GetBindingFlags());

            if (property == null)
                throw new PropertyNotFoundException(MemberName);

            return new ReflectionProperty(property);
        }

        public Type PropertyType
        {
            get { return Find().PropertyTyoe; }
        }
    }
}