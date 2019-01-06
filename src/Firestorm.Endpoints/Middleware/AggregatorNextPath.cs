namespace Firestorm.Endpoints.Web
{
    /// <summary>
    /// The <see cref="INextPath"/> implementation used in <see cref="NextAggregator"/>.
    /// </summary>
    public class AggregatorNextPath : INextPath
    {
        private readonly INamingConventionSwitcher _namingConventionSwitcher;

        public AggregatorNextPath(string dir, INamingConventionSwitcher namingConventionSwitcher)
        {
            _namingConventionSwitcher = namingConventionSwitcher;
            Raw = dir;
        }

        public string Raw { get; }

        public string GetCoded(int dictionaryPrefixLength)
        {
            string subStrRaw = Raw.Substring(dictionaryPrefixLength);
            return _namingConventionSwitcher.ConvertRequestedToCoded(subStrRaw);
        }
    }
}