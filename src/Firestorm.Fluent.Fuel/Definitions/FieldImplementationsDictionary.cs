using System.Collections.Generic;

namespace Firestorm.Fluent.Fuel.Definitions
{
    internal class FieldImplementationsDictionary<TItem> : Dictionary<string, ApiFieldModel<TItem>>
        where TItem : class
    { }
}