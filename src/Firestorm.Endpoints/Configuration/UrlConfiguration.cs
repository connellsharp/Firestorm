namespace Firestorm.Endpoints.Configuration
{
    /// <summary>
    /// The options used to configure the URL paths. 
    /// </summary>
    public class UrlConfiguration
    {
        /// <summary>
        /// A prefix that when applied to an identifier in the URL can return an <see cref="IRestDictionary"/> rather than a single item.
        /// </summary>
        public string DictionaryReferencePrefix { get; set; } = "by_";

        /// <summary>
        /// If true, enables use of an equals sign in the URL to specify an identifier name before navigating into a collection item.
        /// </summary>
        public bool EnableEquals { get; set; } = true;
    }
}