namespace Firestorm
{
    /// <summary>
    /// The type of REST resource.
    /// Directly maps to interfaces that derive from <see cref="IRestResource"/>.
    /// </summary>
    public enum ResourceType
    {
        Scalar,
        Item,
        Dictionary,
        Collection,
        Directory,
    }
}