namespace Firestorm
{
    public enum Assume
    {
        /// <summary>
        /// Assume nothing, believe nobody and check everything.
        /// </summary>
        Nothing,
        
        /// <summary>
        /// Assumes there is only one method that matches the search, which gives a performance gain.
        /// </summary>
        UnambiguousName
    }
}