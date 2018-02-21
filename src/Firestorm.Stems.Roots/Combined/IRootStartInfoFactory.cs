using System;
using System.Collections.Generic;

namespace Firestorm.Stems.Roots.Combined
{
    public interface IRootStartInfoFactory
    {
        IEnumerable<Type> GetStemTypes();
        IRootStartInfo Get(IStemConfiguration stemConfiguration, string startResourceName);
        RestDirectoryInfo CreateDirectoryInfo();
    }
}