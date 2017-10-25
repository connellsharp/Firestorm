namespace Firestorm.Stems.Attributes.Definitions
{
    /// <summary>
    /// Defines how many levels of nesting the field is visible under.
    /// The higher the number, the more layers the field is visible in by default.
    /// </summary>
    /// <remarks>
    /// Consider rename : NestedVisibility
    /// </remarks>
    public enum Display
    {
        /// <summary>
        /// Field is only displayed when explicitly requested.
        /// </summary>
        Hidden = -1,

        /// <summary>
        /// Field is displayed when viewing a single item only.
        /// </summary>
        FullItem = 0,

        /// <summary>
        /// Field is displayed when viewing a collection of items or when referenced as an item in a parent item.
        /// </summary>
        Nested = 1,

        /// <summary>
        /// Field is displayed even when viewing a parent item that contains a collection of these items or a grandparent item.
        /// </summary>
        NestedMany = 2
    }
}