namespace Firestorm.Endpoints
{
    public interface IQueryCreator
    {
        IRestCollectionQuery Create(string queryString);
    }
}