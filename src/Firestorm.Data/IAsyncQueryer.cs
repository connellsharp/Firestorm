using System;
using System.Linq;
using System.Threading.Tasks;

namespace Firestorm.Data
{
    /// <summary>
    /// Contains a method to perform asyncronous queries on an <see cref="IQueryable{T}"/> implementation.
    /// </summary>
    public interface IAsyncQueryer
    {
        /// <remarks>
        /// Same as <see cref="ForEachAsyncDelegate{T}"/>.
        /// </remarks>
        Task ForEachAsync<T>(IQueryable<T> query, Action<T> action);
    }
}