namespace Firestorm.Fluent
{
    /// <summary>
    /// The configuration used to automatically setup an item in <see cref="ApiItemBuilderExtensions.AutoConfigure{TItem}"/>.
    /// </summary>
    public class AutoConfiguration
    {
        public bool AllowWrite { get; set; } = false;
    }
}