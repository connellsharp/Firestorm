namespace Firestorm.Host.Infrastructure
{
    public interface IPreconditions
    {
        string IfMatch { get; }
        string IfNoneMatch { get; }
        string IfModifiedSince { get; }
        string IfUnmodifiedSince { get; }
        string IfRange { get; }
    }
}