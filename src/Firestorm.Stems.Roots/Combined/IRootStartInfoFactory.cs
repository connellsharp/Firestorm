using System;
using System.Collections.Generic;

namespace Firestorm.Stems.Roots.Combined
{
    public interface IRootStartInfoFactory
    {
        IEnumerable<Type> GetStemTypes(IStemsCoreServices stemsServices);
        IRootStartInfo Get(IStemsCoreServices stemsServices, string startResourceName);
        RestDirectoryInfo CreateDirectoryInfo();
    }
}