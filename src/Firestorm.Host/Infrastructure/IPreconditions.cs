namespace Firestorm.Endpoints.Preconditions
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