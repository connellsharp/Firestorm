using System.Collections.Generic;

namespace Firestorm.Endpoints.Formatting.Naming.Conventions
{
    public interface ICaseConvention
    {
        bool IsCase(string casedString);

        IEnumerable<string> Split(string casedString);

        string Make(IEnumerable<string> words);
    }
}