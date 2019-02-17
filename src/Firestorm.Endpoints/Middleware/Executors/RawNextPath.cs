namespace Firestorm.Endpoints
{
    internal class RawNextPath : INextPath
    {
        public RawNextPath(string raw)
        {
            Raw = raw;
        }

        public string Raw { get; }

        public string GetCoded(int offset = 0, int? length = null)
        {
            if (length.HasValue)
                return Raw.Substring(offset, length.Value);
            else
                return Raw.Substring(offset);
        }
    }
}