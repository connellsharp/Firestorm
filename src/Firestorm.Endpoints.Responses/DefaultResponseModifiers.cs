using System;
using System.Collections;
using System.Collections.Generic;

namespace Firestorm.Endpoints.Responses
{
    public class DefaultResponseModifiers : IEnumerable<IResponseModifier>
    {
        private readonly List<IResponseModifier> _list;

        public DefaultResponseModifiers(ResponseConfiguration responseConfiguration)
        {
            _list = new List<IResponseModifier>
            {
                new MainBodyResponseModifier(),
                new FeedbackResponseHeadersModifier(),
                new ErrorResponseModifier()
            };

            switch (responseConfiguration.StatusField)
            {
                case ResponseStatusField.None:
                    break;

                case ResponseStatusField.StatusCode:
                    _list.Add(new StatusCodeResponseModifier
                    {
                        WrapResourceObject = responseConfiguration.WrapResourceObject
                    });
                    break;

                case ResponseStatusField.SuccessBoolean:
                    _list.Add(new SuccessBooleanResponseModifier
                    {
                        WrapResourceObject = responseConfiguration.WrapResourceObject
                    });
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(responseConfiguration.StatusField), "Given StatusField in the response config is invalid.");
            }

            if(responseConfiguration.ShowDeveloperErrors)
                _list.Add(new DeveloperExceptionInfoResponseModifier());

            if(responseConfiguration.Pagination.UseLinkHeaders)
                _list.Add(new PaginationHeadersResponseModifier());

            if(responseConfiguration.Pagination.WrapCollectionResponseBody)
                _list.Add(new PagedBodyResponseModifier());
        }

        public IEnumerator<IResponseModifier> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}