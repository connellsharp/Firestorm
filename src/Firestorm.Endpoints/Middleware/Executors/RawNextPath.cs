namespace Firestorm.Endpoints
{
    internal class RawNextPath : INextPath
    {
        public RawNextPath(string raw)
        {
            Raw = raw;
        }

        public string Raw { get; }
        
        public string GetCoded(int offset = 0)
        {
            return Raw;
        }
    }
}