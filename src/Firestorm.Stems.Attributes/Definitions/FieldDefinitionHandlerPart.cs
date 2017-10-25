using System;
using System.Linq.Expressions;

namespace Firestorm.Stems.Attributes.Definitions
{
    /// <summary>
    /// Part of a <see cref="FieldDefinition{TItem}"/> that can be used to get field values in Firestorm.
    /// </summary>
    public class FieldDefinitionHandlerPart
    {
        /// <summary>
        /// A LINQ expression that defines a property or part of a query.
        /// </summary>
        public LambdaExpression Expression { get; set; }

        /// <summary>
        /// A function to return a method specific to an instance of the given <see cref="Stem"/>.
        /// The function may return a reusable object and ignore the stem argument.
        /// </summary>
        public GetInstanceMethodDelegate GetInstanceMethod { get; set; }

        public delegate Delegate GetInstanceMethodDelegate(object stem);

        public bool HasAnything
        {
            get { return Expression != null || GetInstanceMethod != null; }
        }
    }
}