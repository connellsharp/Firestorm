namespace Firestorm.Endpoints
{
    /// <summary>
    /// An "unsafe" method i.e. one that changes the state of the resource on the server.
    /// Get and Options are considered safe methods and are therefore not in this enum.
    /// </summary>
    public enum UnsafeMethod
    {
        Post,
        Put,
        Patch,
        Delete
    }
}