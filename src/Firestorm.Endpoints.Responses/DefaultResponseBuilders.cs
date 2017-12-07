using System.Collections;
using System.Collections.Generic;

namespace Firestorm.Endpoints.Responses
{
    public class DefaultResponseBuilders : IEnumerable<IResponseBuilder>
    {
        private readonly List<IResponseBuilder> _list;

        public DefaultResponseBuilders(RestEndpointConfiguration endpointConfiguration)
        {
            _list = new List<IResponseBuilder>
            {
                new DirectResponseBuilder(),
                new PaginationHeadersResponseBuilder(),
                new FeedbackResponseHeadersBuilder(),
                new ErrorResponseBuilder()
            };

            if(endpointConfiguration.ShowDeveloperErrors)
                _list.Add(new DeveloperExceptionInfoResponseBuilder());
        }

        public IEnumerator<IResponseBuilder> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}