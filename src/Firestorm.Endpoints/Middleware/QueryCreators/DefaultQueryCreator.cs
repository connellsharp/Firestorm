using Firestorm.Endpoints.Query;

namespace Firestorm.Endpoints.QueryCreators
{
    internal class DefaultQueryCreator : IQueryCreator
    {
        private readonly QueryStringConfiguration _config;

        public DefaultQueryCreator(QueryStringConfiguration config)
        {
            _config = config;
        }

        public IRestCollectionQuery Create(string queryString)
        {
            return new QueryStringCollectionQuery(_config, queryString);
        }
    }
}