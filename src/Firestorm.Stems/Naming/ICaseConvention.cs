using System.Collections.Generic;

namespace Firestorm.Stems.Naming
{
    public interface ICaseConvention
    {
        bool IsCase(string casedString);

        IEnumerable<string> Split(string casedString);

        string Make(IEnumerable<string> words);
    }
}