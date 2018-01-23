using System;
using System.Collections;
using System.Collections.Generic;

namespace Firestorm.Endpoints.Responses
{
    public class DefaultResponseModifiers : IEnumerable<IResponseModifier>
    {
        private readonly List<IResponseModifier> _list;

        public DefaultResponseModifiers(ResponseConfiguruation responseConfiguruation)
        {
            _list = new List<IResponseModifier>
            {
                new MainBodyResponseModifier(),
                new FeedbackResponseHeadersModifier(),
                new ErrorResponseModifier()
            };

            switch (responseConfiguruation.StatusField)
            {
                case ResponseStatusField.None:
                    break;

                case ResponseStatusField.StatusCode:
                    _list.Add(new StatusCodeResponseModifier
                    {
                        WrapResourceObject = responseConfiguruation.WrapResourceObject
                    });
                    break;

                case ResponseStatusField.SuccessBoolean:
                    _list.Add(new SuccessBooleanResponseModifier
                    {
                        WrapResourceObject = responseConfiguruation.WrapResourceObject
                    });
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(responseConfiguruation.StatusField), "Given StatusField in the response config is invalid.");
            }

            if(responseConfiguruation.ShowDeveloperErrors)
                _list.Add(new DeveloperExceptionInfoResponseModifier());

            if(responseConfiguruation.PageConfiguration.UseLinkHeaders)
                _list.Add(new PaginationHeadersResponseModifier());

            if(responseConfiguruation.PageConfiguration.WrapCollectionResponseBody)
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