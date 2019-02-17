using Firestorm.Stems.AutoMap;
using JetBrains.Annotations;

namespace Firestorm.Stems
{
    /// <summary>
    /// Contains the configuration for how an application's Stem objects can be utilised.
    /// </summary>
    public interface IStemsCoreServices
    {
        /// <summary>
        /// Resolves dependencies for constructor parameters on Stem classes.
        /// </summary>
        [CanBeNull] // if no constructor parameters are used on Stem classes.
        IDependencyResolver DependencyResolver { get; }

        /// <summary>
        /// Automatically finds Expressions for Stem properties that do not return Expressions.
        /// </summary>
        [CanBeNull] // if no auto mapped properties are used.
        IPropertyAutoMapper AutoPropertyMapper { get; }

        /// <summary>
        /// Resolves implementations used in 
        /// </summary>
        IServiceGroup ServiceGroup { get; }
    }
}