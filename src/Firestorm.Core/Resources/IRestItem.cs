using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Firestorm
{
    /// <summary>
    /// A single item resource.
    /// </summary>
    public interface IRestItem : IRestResource
    {
        IRestResource GetField([NotNull] string fieldName);

        Task<RestItemData> GetDataAsync([CanBeNull] IEnumerable<string> fieldNames);

        Task<Acknowledgment> EditAsync([NotNull] RestItemData itemData);

        Task<Acknowledgment> DeleteAsync();
    }
}