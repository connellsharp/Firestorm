using System;
using System.Linq;
using System.Threading.Tasks;

namespace Firestorm.Engine
{
    /// <summary>
    /// Interface to manage data transaction lifetime for an <see cref="IEngineRepository{TItem}"/>.
    /// Similar to a Unit Of Work or a Change Tracker.
    /// </summary>
    public interface IDataTransaction : IDisposable
    {
        /// <summary>
        /// Starts a transaction.
        /// </summary>
        void StartTransaction();

        /// <summary>
        /// Commits the local changes to the database.
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// Undos any local changes and forgets them.
        /// </summary>
        Task RollbackAsync();
    }
}