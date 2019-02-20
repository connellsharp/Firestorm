namespace Firestorm.Endpoints.Responses.Pagination
{
    /// <summary>
    /// The method to generate the next and previous page links used in headers and body.
    /// </summary>
    public enum PageNavigationType
    {
        /// <summary>
        /// Uses a basic ?page=2 approach to navigate to the next page.
        /// </summary>
        PageNumber,

        /// <summary>
        /// Uses an offset (e.g. ?skip=50) to navigate to the next page.
        /// </summary>
        Offset,

        /// <summary>
        /// Attempts to use the existing sort order to filter items to only those after the last item (e.g. ?where=id>123)
        /// </summary>
        SortAndFilter
    }
}