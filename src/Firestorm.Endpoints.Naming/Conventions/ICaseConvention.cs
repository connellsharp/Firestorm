using System.Collections.Generic;

namespace Firestorm.Endpoints.Naming
{
    public interface ICaseConvention
    {
        bool IsCase(string casedString);

        IEnumerable<string> Split(string casedString);

        string Make(IEnumerable<string> words);
    }
}