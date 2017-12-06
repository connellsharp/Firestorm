namespace Firestorm.Core.Web
{
    /// <summary>
    /// Links to the previous and next pages.
    /// </summary>
    public class PageLinks
    {
        public PageInstruction NextPath { get; set; }

        public PageInstruction PreviousPath { get; set; }
    }
}