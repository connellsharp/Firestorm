using System;
using System.Reflection;

namespace Firestorm
{
    internal class PropertyFinder : MemberFinder, IPropertyFinder
    {
        public PropertyFinder(Type classType, string propertyName, bool isStatic) 
            : base(classType, propertyName, isStatic)
        {
        }
        
        public IProperty Find()
        {
            PropertyInfo property = ClassType.GetProperty(MemberName, GetBindingFlags());

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