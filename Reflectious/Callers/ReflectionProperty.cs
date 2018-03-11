using System;
using System.Reflection;

namespace Firestorm
{
    public class ReflectionProperty : IProperty
    {
        private readonly PropertyInfo _propertyInfo;

        public ReflectionProperty(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public Type PropertyTyoe => _propertyInfo.PropertyType;

        public object GetValue(object instance)
        {
            return _propertyInfo.GetValue(instance);
        }

        public void SetValue(object instance, object value)
        {
            _propertyInfo.SetValue(instance, value);
        }
    }
}