using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Firestorm
{
    public static class StringExtensions
    {
        /// <summary>
        /// Adds a <see cref="separator"/> character before each capital letter.
        /// </summary>
        /// <remarks>
        /// Idea from http://stackoverflow.com/a/5796427/369247
        /// Should this be in the core? it's related to field naming conventions
        /// </remarks>
        public static string SeparateCamelCase(this string str, string separator = " ", bool makeLowercase = false)
        {
            string replacement = separator + "$1";
            string separated = Regex.Replace(str, "(\\B[A-Z])", replacement);

            if (makeLowercase)
                separated = separated.ToLower();

            return separated;
        }

        /// <summary>
        /// Removes the <see cref="separator"/> characters and capitalises the letter that comes after.
        /// </summary>
        /// <remarks>
        /// Idea from http://stackoverflow.com/a/5796427/369247
        /// Should this be in the core? it's related to field naming conventions
        /// </remarks>
        public static string MakeCamelCase(this string str, char separator = ' ', bool firstCapital = false)
        {
            var builder = new StringBuilder();

            int i = 0;

            if (firstCapital)
            {
                builder.Append(char.ToUpper(str[i]));
                i++;
            }

            while (i < str.Length)
            {
                if (str[i] == separator)
                {
                    i++;
                    builder.Append(char.ToUpper(str[i]));
                }
                else
                {
                    builder.Append(char.ToLower(str[i]));
                }

                i++;
            }

            return builder.ToString();
        }

        // from http://stackoverflow.com/a/4335913/369247
        public static string TrimStart(this string target, string trimString)
        {
            string result = target;
            while (result.StartsWith(trimString, StringComparison.Ordinal))
            {
                result = result.Substring(trimString.Length);
            }

            return result;
        }

        // from http://stackoverflow.com/a/4335913/369247
        public static string TrimEnd(this string target, string trimString)
        {
            string result = target;
            while (result.EndsWith(trimString, StringComparison.Ordinal))
            {
                result = result.Substring(0, result.Length - trimString.Length);
            }

            return result;
        }
    }
}