using System;
using Firestorm.Data;

namespace Firestorm.Stems.Roots.Combined
{
    public interface IRootStartInfo
    {
        Type GetStemType();
        IAxis GetAxis(IStemsCoreServices configuration, IRestUser user);
        IDataSource GetDataSource();
    }
}