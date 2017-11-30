using System;
using Firestorm.Engine.Subs.Context;
using Firestorm.Tests.Engine.Models;

namespace Firestorm.Tests.Stems
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