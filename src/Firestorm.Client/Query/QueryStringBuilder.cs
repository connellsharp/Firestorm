using System;
using System.Text;
using JetBrains.Annotations;

namespace Firestorm.Client.Query
{
    internal class QueryStringBuilder
    {
        private readonly StringBuilder _builder = new StringBuilder();

        public QueryStringBuilder()
        {
        }

        public void AppendPair([NotNull] string key, [CanBeNull] string value)
        {
            AppendPair(key, "=", value);
        }

        public void AppendPair([NotNull] string key, string operatorStr, [CanBeNull] string value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (_builder.Length > 0)
                _builder.Append("&");

            _builder.Append(UrlEncode(key));
            _builder.Append(operatorStr);

            if (value != null)
                _builder.Append(UrlEncode(value));
        }

        private static string UrlEncode(string encodedString)
        {
            return Uri.EscapeDataString(encodedString);
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}