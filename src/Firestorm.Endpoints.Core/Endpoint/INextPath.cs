namespace Firestorm.Endpoints
{
    public interface INextPath
    {
        /// <summary>
        /// Gets the path segment exactly as it was requested, before naming convention switching.
        /// </summary>
        string Raw { get; }

        /// <summary>
        /// Gets the path segment converted to the coded naming convention.
        /// </summary>
        /// <param name="offset">Removes the length from the <see cref="Raw"/> string before converting case.</param>
        string GetCoded(int offset = 0, int? length = null);
    }
}