using System;
using JetBrains.Annotations;

namespace Firestorm.Endpoints
{
    internal static class QueryValidationUtility
    {
        internal static void EnsureValidQuery([CanBeNull] IRestCollectionQuery query)
        {
            if (query == null)
                return;

            if (query.PageInstruction != null && query.PageInstruction.Size <= 0)
                throw new InvalidOperationException("Page size must be set to a value above 0.");
        }
    }
}