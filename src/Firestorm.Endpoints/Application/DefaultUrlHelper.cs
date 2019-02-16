namespace Firestorm.Endpoints.Configuration
{
    public class DefaultUrlHelper : IUrlHelper
    {
        private readonly UrlConfiguration _config;

        public DefaultUrlHelper(UrlConfiguration config)
        {
            _config = config;
        }
        
        public IdentifierInfo GetIdentifierInfo(INextPath path)
        {
            if (!string.IsNullOrEmpty(_config.DictionaryReferencePrefix) && path.Raw.StartsWith(_config.DictionaryReferencePrefix))
            {
                return new IdentifierInfo
                {
                    IsDictionary = true,
                    Value = null,
                    Name = path.GetCoded(_config.DictionaryReferencePrefix.Length)
                };
            }
            
            if(_config.EnableEquals && path.Raw.Contains("="))
            {
                // see https://stackoverflow.com/a/20386425/369247
                
                string[] split = path.Raw.Split(new[] { '=' }, 2);
                
                return new IdentifierInfo
                {
                    IsDictionary = false,
                    Value = split[1],
                    Name = split[0]
                };
            }

            return new IdentifierInfo
            {
                IsDictionary = false,
                Value = path.Raw,
                Name = null
            };
        }
    }
}