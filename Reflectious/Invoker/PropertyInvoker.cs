using System.Reflection;

namespace Firestorm
{
    public class PropertyInvoker
    {
        private readonly object _instance;
        private readonly PropertyInfo _property;

        protected PropertyInvoker(object instance, PropertyInfo property)
        {
            _instance = instance;
            _property = property;
        }

        public object GetValue()
        {
            return _property.GetValue(_instance);
        }

        public void SetValue(object value)
        {
            _property.SetValue(_instance, value);
        }
    }
}