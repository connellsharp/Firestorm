namespace Firestorm.Rest.Web
{
    /// <summary>
    /// The body of a request from a Firestorm API endpoint.
    /// The can be either a collection, item or scalar value.
    /// </summary>
    /// <remarks>
    /// The inheritance hierarchy here is very similar to <see cref="IRestResource"/>.
    /// </remarks>
    public abstract class ResourceBody
    {
        internal ResourceBody()
        { }

        public abstract ResourceType ResourceType { get; }

        public abstract object GetObject();
    }
}