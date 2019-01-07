namespace Firestorm.Endpoints.Configuration
{
    /// <summary>
    /// The system-wide configuration defining how pagination can work.
    /// </summary>
    public class PageConfiguration
    {
        public int MaxPageSize { get; set; } = 100;

        /// <summary>
        /// True means a Link header is added to collection responses containing links to the next and previous pages.
        /// </summary>
        public bool UseLinkHeaders { get; set; } = true;

        /// <summary>
        /// True means the collection response will be wrapped in an object that also has a property for the page navigation.
        /// </summary>
        public bool WrapCollectionResponseBody { get; set; }

        /// <summary>
        /// The method to generate the next and previous page links used in headers and body.
        /// </summary>
        public PageNavigationType SuggestedNavigationType { get; set; }
    }
}