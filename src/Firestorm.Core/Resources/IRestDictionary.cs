using System.Threading.Tasks;

namespace Firestorm
{
    /// <summary>
    /// A collection of identifier to <see cref="IRestItem"/> pairs.
    /// </summary>
    /// <remarks>
    /// Behaves similarly to <see cref="IRestCollection"/>, but contains unique keys for each item.
    /// </remarks>
    public interface IRestDictionary : IRestResource
    {
        Task<RestDictionaryData> QueryDataAsync(IRestCollectionQuery query);

        IRestItem GetItem(string identifier);
    }
}