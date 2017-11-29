using System.Collections.Generic;
using System.Text;
using Firestorm.Core.Web;

namespace Firestorm.Endpoints.Start
{
    public class LinkHeaderBuilder
    {
        private readonly Dictionary<string, string> _links = new Dictionary<string, string>();

        public void AddDetails(PageLinks pageLinks)
        {
            _links.Add("next", pageLinks.NextPath);
            _links.Add("prev", pageLinks.PreviousPath);
        }

        public void SetHeaders(IHttpRequestHandler requestHandler)
        {
            string headerValue = GetHeaderValue(requestHandler.ResourcePath);
            requestHandler.SetResponseHeader("Link", headerValue);
        }

        public string GetHeaderValue(string resourcePath)
        {
            var builder = new StringBuilder();

            foreach (var link in _links)
            {
                builder.AppendFormat("<{0}?{1}>;rel=\"{2}\", ", resourcePath, link.Value, link.Key);
            }

            string headerValue = builder.ToString(0, builder.Length - 2);
            return headerValue;
        }
    }
}