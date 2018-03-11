using System;
using System.Reflection;

namespace Firestorm
{
    public interface IProperty
    {
        Type PropertyTyoe { get; }
        
        object GetValue(object instance);
        
        void SetValue(object instance, object value);
    }
}