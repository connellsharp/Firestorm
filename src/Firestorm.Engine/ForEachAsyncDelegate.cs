using System;
using System.Linq;
using System.Threading.Tasks;

namespace Firestorm.Engine
{
    /// <summary>
    /// The method used to loop over an <see cref="IQueryable{T}"/>, performing an <see cref="Action{T}"/> asynchronously.
    /// </summary>
    public delegate Task ForEachAsyncDelegate<T>(IQueryable<T> source, Action<T> action);

    // Same as Func<IQueryable<object>, Action<object>, Task>
}