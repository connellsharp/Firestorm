using System;
using System.Collections.Generic;

namespace Firestorm
{
    /// <summary>
    /// Defines how to import objects into a <see cref="RestItemData"/> object.
    /// </summary>
    public interface IRestItemDataImporter
    {
        bool CanImport(object obj);

        Type GetType(object obj);

        IEnumerable<KeyValuePair<string, object>> GetValues(object obj);
    }
}