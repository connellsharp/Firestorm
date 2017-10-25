using System;
using Firestorm.Core;

namespace Firestorm.Endpoints
{
    internal static class QueryValidationUtility
    {
        internal static void EnsureValidQuery(IRestCollectionQuery query)
        {
            if (query.PageSize <= 0)
                throw new InvalidOperationException("PageSize must be set to a value above 0.");
        }
    }
}