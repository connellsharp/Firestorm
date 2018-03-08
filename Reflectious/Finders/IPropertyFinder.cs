using System;
using System.Reflection;

namespace Firestorm
{
    public interface IPropertyFinder
    {
        PropertyInfo Find();
        Type PropertyType { get; }
    }
}