using Firestorm.Data;
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
        /// Contains the services used to manage entities in a database.
        /// </summary>
        IDataContext<TItem> Data { get; }

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