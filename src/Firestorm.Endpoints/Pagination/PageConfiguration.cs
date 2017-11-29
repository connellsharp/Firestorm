namespace Firestorm.Endpoints.Pagination
{
    /// <summary>
    /// The system-wide configuration defining how pagination can work.
    /// </summary>
    public class PageConfiguration
    {
        public int MaxPageSize { get; set; } = 100;

        public bool UseLinkHeaders { get; set; }

        public PageNavigationType SuggestedNavigationType { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PageNavigationType
    {
        SortAndFilter,
        PageNumber,
        Offset
    }
}