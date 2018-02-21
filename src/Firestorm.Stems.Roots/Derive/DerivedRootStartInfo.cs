using System;
using Firestorm.Data;
using Firestorm.Stems.Roots.Combined;

namespace Firestorm.Stems.Roots.Derive
{
    public class DerivedRootStartInfo : IRootStartInfo
    {
        private readonly Root _root;

        public DerivedRootStartInfo(Root root)
        {
            _root = root;
        }

        public Type GetStemType()
        {
            return _root.StartStemType;
        }

        public IAxis GetAxis(IStemConfiguration configuration, IRestUser user)
        {
            _root.Configuration = configuration;
            _root.User = user;

            return _root;
        }

        public IDataSource GetDataSource()
        {
            return new RootDataSource(_root);
        }
    }
}