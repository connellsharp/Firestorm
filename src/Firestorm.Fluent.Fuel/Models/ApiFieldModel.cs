using System;
using System.Linq.Expressions;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Context;

namespace Firestorm.Fluent.Fuel.Models
{
    internal class ApiFieldModel<TItem>
        where TItem : class
    {
        public LambdaExpression Expression { get; set; }

        public string Name { get; set; }

        public IFieldReader<TItem> Reader { get; set; }

        public IFieldWriter<TItem> Writer { get; set; }

        public IFieldDescription Description { get; set; }

        public IFieldResourceGetter<TItem> ResourceGetter { get; set; }

        public IItemLocator<TItem> Locator { get; set; }
    }
}