using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace Firestorm.Endpoints.Formatting.Naming
{
    /// <summary>
    /// Exposes and implements <see cref="IRestCollectionQuery"/> to wrap one instance and switch the names of the fields.
    /// </summary>
    /// <remarks>
    /// This pattern again. Not sure if I like it. But it works for this naming convention stuff.
    /// </remarks>
    public class NameSwitcherQuery : IRestCollectionQuery
    {
        private readonly IRestCollectionQuery _underlyingQuery;
        private readonly INamingConventionSwitcher _switcher;

        public NameSwitcherQuery([NotNull] IRestCollectionQuery underlyingQuery, [NotNull] INamingConventionSwitcher switcher)
        {
            Debug.Assert(underlyingQuery != null);
            Debug.Assert(switcher != null);

            _underlyingQuery = underlyingQuery;
            _switcher = switcher;
        }

        public IEnumerable<string> SelectFields => _underlyingQuery.SelectFields.Select(_switcher.ConvertRequestedToCoded);

        public IEnumerable<FilterInstruction> FilterInstructions =>
            _underlyingQuery.FilterInstructions.Select(fi => new FilterInstruction(_switcher.ConvertRequestedToCoded(fi.FieldName), fi.Operator, fi.ValueString));

        public IEnumerable<SortInstruction> SortInstructions =>
            _underlyingQuery.SortInstructions.Select(fi => new SortInstruction(_switcher.ConvertRequestedToCoded(fi.FieldName), fi.Direction));

        public PageInstruction PageInstruction => _underlyingQuery.PageInstruction;
    }
}