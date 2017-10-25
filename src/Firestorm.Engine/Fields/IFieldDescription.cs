namespace Firestorm.Engine.Fields
{
    /// <summary>
    /// A description (and possibly example) of a field
    /// </summary>
    public interface IFieldDescription
    {
        string Description { get; }

        object Example { get; }
    }
}