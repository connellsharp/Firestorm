using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;

namespace Firestorm.Engine.Fields
{
    /// <summary>
    /// A mapping of given API field names to <see cref="IFieldReader{TItem}"/> and <see cref="IFieldWriter{TItem}"/> objects.
    /// </summary>
    public interface IFieldProvider<in TItem>
        where TItem : class
    {
        /// <summary>
        /// Returns the default field names when they are not explicitly specified.
        /// </summary>
        /// <param name="nestedBy">
        /// The number of ancestor levels from the item up to the requested resource.
        /// Higher numbers mean more nesting and should return fewer fields.
        /// <example> 0 = the item, 1 = a parent item or collection.</example>
        /// </param>
        IEnumerable<string> GetDefaultNames(int nestedBy);

        /// <summary>
        /// Returns true if the specified <see cref="fieldName"/> has a reader and/or writer.
        /// </summary>
        bool FieldExists(string fieldName);

        [CanBeNull]
        IRestResource GetFullResource(string fieldName, IDeferredItem<TItem> item, IDataTransaction dataTransaction);

        [CanBeNull]
        IFieldReader<TItem> GetReader(string fieldName);

        [CanBeNull]
        IFieldWriter<TItem> GetWriter(string fieldName);

        [CanBeNull]
        IFieldDescription GetDescription(string fieldName, CultureInfo cultureInfo);
    }
}