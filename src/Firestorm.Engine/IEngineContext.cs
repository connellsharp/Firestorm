using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;

namespace Firestorm.Engine
{
    /// <summary>
    /// Contains all the objects required for a collection or item to run in the Firestorm Engine.
    /// </summary>
    public interface IEngineContext<TItem>
        where TItem : class
    {
        /// <summary>
        /// Manages the data transaction lifetime for an <see cref="IEngineRepository{TItem}"/>.
        /// </summary>
        IDataTransaction Transaction { get; }

        /// <summary>
        /// Manages data storage.
        /// </summary>
        IEngineRepository<TItem> Repository { get; }

        /// <summary>
        /// Provides data about URL paths used to identify items within a collection.
        /// </summary>
        IIdentifierProvider<TItem> Identifiers { get; }

        /// <summary>
        /// Provides methods to get <see cref="IFieldReader{TItem}"/> and <see cref="IFieldWriter{TItem}"/> objects from given field names.
        /// </summary>
        IFieldProvider<TItem> Fields { get; }

        /// <summary>
        /// Checks different levels for authorization to the requested resource.
        /// </summary>
        IAuthorizationChecker<TItem> AuthorizationChecker { get; }
    }
}