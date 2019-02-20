using JetBrains.Annotations;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// A breakdown of information in the identifier directory of a URL path.
    /// </summary>
    public class IdentifierPathInfo
    {
        public string Value { get; set; }
        
        [CanBeNull]
        public string Name { get; set; }
        
        public bool IsDictionary { get; set; }
    }
}