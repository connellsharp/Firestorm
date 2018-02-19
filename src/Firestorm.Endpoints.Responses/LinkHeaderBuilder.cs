using System.Collections.Generic;
using System.Text;
using Firestorm.Core.Web;

namespace Firestorm.Endpoints.Responses
{
    internal class LinkHeaderBuilder
    {
        private readonly Dictionary<string, string> _links = new Dictionary<string, string>();
        private readonly IUrlCalculator _urlCalculator;

        internal LinkHeaderBuilder(IUrlCalculator urlCalculator)
        {
            _urlCalculator = urlCalculator;
        }

        public void AddDetails(PageLinks pageLinks)
        {
            if (pageLinks == null)
                return;

            if (pageLinks.Next != null)
                _links.Add("next", _urlCalculator.GetPageUrl(pageLinks.Next));

            if (pageLinks.Previous != null)
                _links.Add("prev", _urlCalculator.GetPageUrl(pageLinks.Previous));
        }

        public void SetHeaders(Response response)
        {
            string headerValue = GetHeaderValue();

            if (headerValue != null)
                response.Headers["Link"] = headerValue;
        }

        public string GetHeaderValue()
        {
            var builder = new StringBuilder();

            foreach (var link in _links)
            {
                builder.AppendFormat("<{0}>;rel=\"{1}\", ", link.Value, link.Key);
            }

            if (builder.Length == 0)
                return null;

            string headerValue = builder.ToString(0, builder.Length - 2);
            return headerValue;
        }
    }
}