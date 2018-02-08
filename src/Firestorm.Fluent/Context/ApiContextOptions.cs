namespace Firestorm.Fluent
{
    /// <summary>
    /// The options to be used by an <see cref="ApiContext"/>.
    /// </summary>
    public class ApiContextOptions
    {
        public AutoConfigureMode AutoConfigureMode { get; set; }
    }

    public enum AutoConfigureMode
    {
        ReadOnly,
        ReadWrite
    }
}