using System;
using JetBrains.Annotations;

namespace Firestorm.Client
{
    public static class UriUtilities
    {
        public static string AppendQueryString(string path, [CanBeNull] string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
                return path;

            return string.Format(path.Contains("?") ? "{0}&{1}" : "{0}?{1}", path, queryString);
        }

        public static string AppendPath(string path, [NotNull] string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentException("Identifier cannot be null or empty.", nameof(identifier));

            return string.Format("{0}/{1}", path.TrimEnd('/'), identifier);
        }
    }
}