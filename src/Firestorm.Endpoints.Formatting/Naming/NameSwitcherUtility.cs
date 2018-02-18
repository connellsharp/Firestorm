using System;
using JetBrains.Annotations;

namespace Firestorm.Endpoints.Formatting.Naming
{
    /// <summary>
    /// Utility to expose the NameSwitcher wrappers.
    /// </summary>
    public static class NameSwitcherUtility
    {
        public static IRestCollectionQuery TryWrapQuery([NotNull] IRestCollectionQuery underlyingQuery, [CanBeNull] INamingConventionSwitcher nameSwitcher)
        {
            if (underlyingQuery == null)
                throw new ArgumentNullException(nameof(underlyingQuery));

            if (nameSwitcher != null)
                return new NameSwitcherQuery(underlyingQuery, nameSwitcher);

            return underlyingQuery;
        }
    }
}