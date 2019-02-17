using Firestorm.Endpoints.Configuration;

namespace Firestorm.Endpoints
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

        public string GetCoded(int offset, int? length)
        {
            string subStrRaw = length.HasValue
                ? Raw.Substring(offset, length.Value)
                : Raw.Substring(offset);
            
            return _namingConventionSwitcher.ConvertRequestedToCoded(subStrRaw);
        }
    }
}