namespace Firestorm.Endpoints
{
    public class DefaultUrlHelper : IUrlHelper
    {
        private readonly UrlConfiguration _config;

        public DefaultUrlHelper(UrlConfiguration config)
        {
            _config = config;
        }
        
        public IdentifierPathInfo GetIdentifierInfo(INextPath path)
        {
            if (!string.IsNullOrEmpty(_config.DictionaryReferencePrefix) && path.Raw.StartsWith(_config.DictionaryReferencePrefix))
            {
                return new IdentifierPathInfo
                {
                    IsDictionary = true,
                    Value = null,
                    Name = path.GetCoded(_config.DictionaryReferencePrefix.Length)
                };
            }

            int equalsIndex = path.Raw.IndexOf('=');
            if(_config.EnableEquals && equalsIndex > 0)
            {
                // see https://stackoverflow.com/a/20386425/369247
                                
                return new IdentifierPathInfo
                {
                    IsDictionary = false,
                    Value = path.GetCoded(equalsIndex + 1),
                    Name = path.GetCoded(0, equalsIndex)
                };
            }

            return new IdentifierPathInfo
            {
                IsDictionary = false,
                Value = path.Raw,
                Name = null
            };
        }
    }
}