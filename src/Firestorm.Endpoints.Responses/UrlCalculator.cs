using System.Text;

namespace Firestorm.Endpoints.Responses
{
    internal class UrlCalculator : IUrlCalculator
    {
        private readonly string _resourcePath;

        public UrlCalculator(string resourcePath)
        {
            _resourcePath = resourcePath;
        }

        public string GetPageUrl(PageInstruction pageInstruction)
        {
            var builder = new StringBuilder();
            builder.Append(_resourcePath);
            builder.Append('?');

            if (pageInstruction.PageNumber.HasValue)
                builder.AppendFormat("page={0}&", pageInstruction.PageNumber.Value);

            if (pageInstruction.Offset.HasValue)
                builder.AppendFormat("offset={0}&", pageInstruction.Offset.Value);

            if (pageInstruction.Size.HasValue)
                builder.AppendFormat("size={0}&", pageInstruction.Size.Value);

            return builder.ToString(0, builder.Length - 1);
        }

        public string GetCreatedUrl(object newIdentifier)
        {
            return string.Format("{0}/{1}", _resourcePath.TrimEnd('/'), newIdentifier);
        }
    }
}