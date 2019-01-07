using System.Linq;

namespace Firestorm.Engine.Deferring
{
    /// <summary>
    /// An <see cref="IQueryable"/> implementation where the response is expected to contain a single element.</summary>
    public interface IQueryableSingle<out T> : IQueryable<T>
    {
    }
}