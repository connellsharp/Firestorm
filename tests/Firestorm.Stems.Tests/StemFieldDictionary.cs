using System;
using Firestorm.Engine.Subs.Context;
using Firestorm.Testing.Engine;

namespace Firestorm.Stems.Tests
{
    public class StemFieldDictionary<TItem> : FieldDictionary<TItem>, ILocatableFieldProvider<TItem>
        where TItem : class
    {
        public IItemLocator<TItem> GetLocator(string apiFieldName)
        {
            throw new NotImplementedException("Test class does not implement locators.");
        }
    }
}