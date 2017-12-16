using System;
using System.Collections;
using System.Collections.Generic;

namespace Firestorm.Endpoints.Responses
{
    public class DefaultResponseBuilders : IEnumerable<IResponseBuilder>
    {
        private readonly List<IResponseBuilder> _list;

        public DefaultResponseBuilders(ResponseConfiguruation responseConfiguruation)
        {
            _list = new List<IResponseBuilder>
            {
                new MainBodyResponseBuilder(),
                new PaginationHeadersResponseBuilder(),
                new FeedbackResponseHeadersBuilder(),
                new ErrorResponseBuilder()
            };

            switch (responseConfiguruation.StatusField)
            {
                case ResponseStatusField.None:
                    break;

                case ResponseStatusField.StatusCode:
                    _list.Add(new StatusCodeResponseBuilder
                    {
                        WrapResourceObject = responseConfiguruation.WrapResourceObject
                    });
                    break;

                case ResponseStatusField.SuccessBoolean:
                    _list.Add(new SuccessBooleanResponseBuilder
                    {
                        WrapResourceObject = responseConfiguruation.WrapResourceObject
                    });
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(responseConfiguruation.StatusField), "Given StatusField in the response config is invalid.");
            }

            if(responseConfiguruation.ShowDeveloperErrors)
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