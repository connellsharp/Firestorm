using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Firestorm
{
    public class PropertyReflector : PropertyReflector<object>
    {
        internal PropertyReflector(object instance, IPropertyFinder propertyFinder) 
            : base(instance, propertyFinder)
        {
        }

        public PropertyReflector<TInstance> WithInstanceType<TInstance>()
        {
            if (!(Instance is TInstance castedInstance))
                throw new IncorrectInstanceTypeException();
            
            return new PropertyReflector<TInstance>(castedInstance, PropertyFinder);
        }
    }

    public class PropertyReflector<TInstance> : PropertyReflector<TInstance, object>
    {
        internal PropertyReflector(TInstance instance, IPropertyFinder propertyFinder) 
            : base(instance, propertyFinder)
        {
        }

        public PropertyReflector<TInstance, TProperty> OfType<TProperty>()
        {
            if (PropertyFinder.PropertyType != typeof(TProperty))
                throw new IncorrectPropertyTypeException();
            
            return new PropertyReflector<TInstance, TProperty>(Instance, PropertyFinder);
        }
    }
    
    public class PropertyReflector<TInstance, TProperty>
    {
        protected readonly TInstance Instance;
        internal readonly IPropertyFinder PropertyFinder;

        internal PropertyReflector(TInstance instance, IPropertyFinder propertyFinder) 
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