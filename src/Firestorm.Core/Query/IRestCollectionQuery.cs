using System.Collections.Generic;
using JetBrains.Annotations;

namespace Firestorm
{
    /// <summary>
    /// Defines the query parameters to select, filter and sort a <see cref="IRestCollection"/>.
    /// </summary>
    public interface IRestCollectionQuery
    {
        [CanBeNull]
        IEnumerable<string> SelectFields { get; }

        [CanBeNull]
        IEnumerable<FilterInstruction> FilterInstructions { get; }

        [CanBeNull]
        IEnumerable<SortIntruction> SortIntructions { get; }

        int PageSize { get; }
    }
}