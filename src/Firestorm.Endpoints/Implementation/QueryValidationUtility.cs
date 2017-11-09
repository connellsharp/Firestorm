using System;
using Firestorm.Core;

namespace Firestorm.Endpoints
{
    internal static class QueryValidationUtility
    {
        internal static void EnsureValidQuery(IRestCollectionQuery query)
        {
            if (query.PageInstruction.Size <= 0)
                throw new InvalidOperationException("Page size must be set to a value above 0.");
        }
    }
}