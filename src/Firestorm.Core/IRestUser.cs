namespace Firestorm
{
    /// <summary>
    /// A user of a Firestorm API.
    /// </summary>
    /// <remarks>
    /// Not sure if this should be in the Core as it's not used by the Engine at all. Maybe Core.Web?
    /// </remarks>
    public interface IRestUser
    {
        string Username { get; }

        bool IsAuthenticated { get; }

        bool IsInRole(string role);
    }
}