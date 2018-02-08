namespace Firestorm.Fluent
{
    /// <summary>
    /// The options to be used by an <see cref="ApiContext"/>.
    /// </summary>
    public class ApiContextOptions
    {
        /// <summary>
        /// The configuration used to configure the <see cref="ApiRoot{TItem}"/> properties.
        /// </summary>
        public AutoConfiguration RootConfiguration { get; set; } = new AutoConfiguration();
    }
}