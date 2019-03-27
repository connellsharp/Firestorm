using System;
using System.Collections.Generic;

namespace Firestorm.Stems.Roots.Combined
{
    public interface IRootStartInfoFactory
    {
        IEnumerable<Type> GetStemTypes(IStemsCoreServices services);
        IRootStartInfo Get(IStemsCoreServices stemsServices, string startResourceName);
        RestDirectoryInfo CreateDirectoryInfo();
    }
}