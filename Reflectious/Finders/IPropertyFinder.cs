using System;

namespace Firestorm
{
    internal interface IPropertyFinder
    {
        IProperty Find();
        Type PropertyType { get; }
    }
}