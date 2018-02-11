using System;
using System.Collections.Generic;
using System.Linq;

namespace Firestorm.Stems.Naming
{
    public class CapsUnderscoreConvention : ICaseConvention
    {
        public bool IsCase(string casedString)
        {
            return casedString.All(char.IsUpper);
        }

        public IEnumerable<string> Split(string casedString)
        {
            return casedString.Split('_');
        }

        public string Make(IEnumerable<string> words)
        {
            return string.Join("_", words.Select(w => w.ToUpper()));
        }
    }
}