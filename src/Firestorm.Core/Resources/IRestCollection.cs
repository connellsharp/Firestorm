using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Firestorm
{
    /// <summary>
    /// A collection of <see cref="IRestItem"/> that can be identified by a unique identifier.
    /// </summary>
    public interface IRestCollection : IRestResource
    {
        Task<RestCollectionData> QueryDataAsync([CanBeNull] IRestCollectionQuery query);

        IRestItem GetItem(string identifier, [CanBeNull] string identifierName = null);

        IRestDictionary ToDictionary(string identifierName);

        Task<CreatedItemAcknowledgment> AddAsync(RestItemData itemData);
    }
}