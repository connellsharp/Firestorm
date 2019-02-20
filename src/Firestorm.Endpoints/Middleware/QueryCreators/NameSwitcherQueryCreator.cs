using Firestorm.Endpoints.Formatting.Naming;

namespace Firestorm.Endpoints.QueryCreators
{
    /// <summary>
    /// Decorates a <see cref="IQueryCreator"/> allowing switching names of fields.
    /// </summary>
    internal class NameSwitcherQueryCreator : IQueryCreator
    {
        private readonly IQueryCreator _underlyingCreator;
        private readonly INamingConventionSwitcher _nameSwitcher;

        public NameSwitcherQueryCreator(IQueryCreator underlyingCreator, INamingConventionSwitcher nameSwitcher)
        {
            _underlyingCreator = underlyingCreator;
            _nameSwitcher = nameSwitcher;
        }

        public IRestCollectionQuery Create(string queryString)
        {
            var query = _underlyingCreator.Create(queryString);
            return NameSwitcherUtility.TryWrapQuery(query, _nameSwitcher);
        }
    }
}