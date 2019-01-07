using System.Linq;
using System.Threading.Tasks;

namespace Firestorm.Engine.Fields
{
    /// <summary>
    /// Replaces field value results after the query has been executed.
    /// Used for optimization in complex fields.
    /// </summary>
    public interface IFieldValueReplacer<in TItem>
    {
        /// <summary>
        /// Loads any values that are needed to get the replacements.
        /// </summary>
        Task LoadAsync(IQueryable<TItem> itemsQuery);

        /// <summary>
        /// Gets the replacement object to actually return to the API.
        /// </summary>
        /// <param name="dbValue">The original database value from the fields getter expression.</param>
        object GetReplacement(object dbValue);
    }
}