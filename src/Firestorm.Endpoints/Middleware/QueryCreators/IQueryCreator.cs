namespace Firestorm.Endpoints.QueryCreators
{
    public interface IQueryCreator
    {
        IRestCollectionQuery Create(string queryString);
    }
}