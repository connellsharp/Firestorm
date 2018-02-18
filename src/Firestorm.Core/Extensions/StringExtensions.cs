using System;

namespace Firestorm
{
    public static class StringExtensions
    {
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