namespace Firestorm.Endpoints.Pagination
{
    /// <summary>
    /// The method to generate the next and previous page links used in headers and body.
    /// </summary>
    public enum PageNavigationType
    {
        SortAndFilter,
        PageNumber,
        Offset
    }
}