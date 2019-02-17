using JetBrains.Annotations;

namespace Firestorm.Endpoints.Configuration
{
    public class IdentifierInfo
    {
        public string Value { get; set; }
        
        [CanBeNull]
        public string Name { get; set; }
        
        public bool IsDictionary { get; set; }
    }
}