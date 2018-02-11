using System.Collections.Generic;
using System.Linq;

namespace Firestorm.Stems.Naming
{
    internal class SnakeCaseConvention : ICaseConvention
    {
        public bool IsCase(string casedString)
        {
            return casedString.IndexOf('_') >= 0;
        }

        public IEnumerable<string> Split(string casedString)
        {
            return casedString.Split('_');
        }

        public string Make(IEnumerable<string> words)
        {
            return string.Join("_", words.Select(w => w.ToLower()));
        }
    }

    internal class KebabCaseConvention : ICaseConvention
    {
        public bool IsCase(string casedString)
        {
            return casedString.IndexOf('-') >= 0;
        }

        public IEnumerable<string> Split(string casedString)
        {
            return casedString.Split('-');
        }

        public string Make(IEnumerable<string> words)
        {
            return string.Join("-", words.Select(w => w.ToLower()));
        }
    }
}