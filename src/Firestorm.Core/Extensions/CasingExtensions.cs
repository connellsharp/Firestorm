using System.Text;
using System.Text.RegularExpressions;

namespace Firestorm
{
    /// <summary>
    /// Extensions to do with casing conventions.
    /// </summary>
    /// <remarks>
    /// Should this be in the core? it's related to field naming conventions. Perhaps Core.Web ?
    /// </remarks>
    public static class CasingExtensions
    {
        /// <summary>
        /// Adds a <see cref="separator"/> character before each capital letter.
        /// </summary>
        /// <remarks>
        /// Idea from http://stackoverflow.com/a/5796427/369247
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
    }
}