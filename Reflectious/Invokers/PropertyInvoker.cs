using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Firestorm
{
    public class PropertyInvoker : PropertyInvoker<object>
    {
        internal PropertyInvoker(object instance, IPropertyFinder propertyFinder) 
            : base(instance, propertyFinder)
        {
        }

        public PropertyInvoker<TInstance> WithInstanceType<TInstance>()
        {
            if (!(Instance is TInstance castedInstance))
                throw new IncorrectInstanceTypeException();
            
            return new PropertyInvoker<TInstance>(castedInstance, PropertyFinder);
        }
    }

    public class PropertyInvoker<TInstance> : PropertyInvoker<TInstance, object>
    {
        internal PropertyInvoker(TInstance instance, IPropertyFinder propertyFinder) 
            : base(instance, propertyFinder)
        {
        }

        public PropertyInvoker<TInstance, TProperty> OfType<TProperty>()
        {
            if (PropertyFinder.PropertyType != typeof(TProperty))
                throw new IncorrectPropertyTypeException();
            
            return new PropertyInvoker<TInstance, TProperty>(Instance, PropertyFinder);
        }
    }
    
    public class PropertyInvoker<TInstance, TProperty>
    {
        protected readonly TInstance Instance;
        internal readonly IPropertyFinder PropertyFinder;

        internal PropertyInvoker(TInstance instance, IPropertyFinder propertyFinder) 
        {
            Instance = instance;
            PropertyFinder = propertyFinder;
        }

        public TProperty GetValue()
        {
            IProperty property = PropertyFinder.Find();
            return (TProperty)property.GetValue(Instance);
        }

        public void SetValue(TProperty value)
        {
            IProperty property = PropertyFinder.Find();
            property.SetValue(Instance, value);
        }
    }
}