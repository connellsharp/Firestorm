﻿using System.Collections.Generic;
using System.Text;
using Firestorm.Core.Web;

namespace Firestorm.Endpoints.Start
{
    public class LinkHeaderBuilder
    {
        private readonly Dictionary<string, string> _links = new Dictionary<string, string>();

        public void AddDetails(PageLinks pageLinks)
        {
            if (pageLinks == null)
                return;

            if (pageLinks.NextPath != null)
                _links.Add("next", pageLinks.NextPath);

            if (pageLinks.PreviousPath != null)
                _links.Add("prev", pageLinks.PreviousPath);
        }

        public void SetHeaders(IHttpRequestHandler requestHandler)
        {
            string headerValue = GetHeaderValue(requestHandler.ResourcePath);

            if (headerValue != null)
                requestHandler.SetResponseHeader("Link", headerValue);
        }

        public string GetHeaderValue(string resourcePath)
        {
            var builder = new StringBuilder();

            foreach (var link in _links)
            {
                builder.AppendFormat("<{0}?{1}>;rel=\"{2}\", ", resourcePath, link.Value, link.Key);
            }

            if (builder.Length == 0)
                return null;

            string headerValue = builder.ToString(0, builder.Length - 2);
            return headerValue;
        }
    }
}