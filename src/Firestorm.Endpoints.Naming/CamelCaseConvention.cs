using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Firestorm.Stems.Naming
{
    internal class CamelCaseConvention : ICaseConvention
    {
        public bool IsCase(string casedString)
        {
            return casedString.IndexOfAny(new[] { '_', '-' }) == -1 && char.IsLower(casedString[0]);
        }

        public IEnumerable<string> Split(string casedString)
        {
            // regex from http://stackoverflow.com/a/155487/369247

            foreach (Match match in Regex.Matches(casedString, "([A-Z]+(?=$|[A-Z][a-z])|[A-Z]?[a-z]+)"))
            {
                yield return match.Groups[0].Value;
            }
        }

        public string Make(IEnumerable<string> words)
        {
            StringBuilder builder = new StringBuilder();

            bool first = true;

            foreach (string word in words)
            {
                if (first)
                {
                    builder.Append(word.ToLower());
                    first = false;
                }
                else if (PascalCaseConvention.KnownTwoLetterAcronyms.Any(ka => string.Equals(ka, word, StringComparison.OrdinalIgnoreCase)))
                {
                    builder.Append(word.ToUpper());
                }
                else
                {
                    builder.Append(word.Substring(0, 1).ToUpper());
                    builder.Append(word.Substring(1, word.Length - 1).ToLower());
                }
            }

            return builder.ToString();
        }
    }
}