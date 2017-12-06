namespace Firestorm.Core.Web
{
    /// <summary>
    /// Links to the previous and next pages.
    /// </summary>
    public class PageLinks
    {
        public PageInstruction Next { get; set; }

        public PageInstruction Previous { get; set; }
    }
}