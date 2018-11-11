using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Firestorm
{
    /// <summary>
    /// An entire REST directory. Usually the root of the API.
    /// </summary>
    public interface IRestDirectory : IRestResource
    {
        IRestResource GetChild([NotNull] string startResourceName);

        Task<RestDirectoryInfo> GetInfoAsync();
    }
}