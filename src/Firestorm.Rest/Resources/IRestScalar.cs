using System.Threading.Tasks;

namespace Firestorm
{
    /// <summary>
    /// A scalar value resource (e.g. a <see cref="string"/> or <see cref="int"/>).
    /// </summary>
    public interface IRestScalar : IRestResource
    {
        Task<object> GetAsync();

        Task<Acknowledgment> EditAsync(object value);
    }
}