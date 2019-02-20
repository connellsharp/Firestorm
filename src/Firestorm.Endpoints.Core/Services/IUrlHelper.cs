namespace Firestorm.Endpoints
{
    public interface IUrlHelper
    {
        IdentifierPathInfo GetIdentifierInfo(INextPath path);
    }
}