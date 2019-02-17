namespace Firestorm.Endpoints.Configuration
{
    public interface IUrlHelper
    {
        IdentifierInfo GetIdentifierInfo(INextPath path);
    }
}