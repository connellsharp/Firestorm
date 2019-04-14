using System;
using System.Collections.Generic;
using System.Linq;
using Firestorm.Stems.Fuel.Resolving;
using Firestorm.Stems.Roots.Combined;

namespace Firestorm.Stems.Roots.Derive
{
    public class DerivedRootStartInfoFactory : IRootStartInfoFactory
    {
        private readonly NamedTypeDictionary _rootTypeDictionary;

        public DerivedRootStartInfoFactory(ITypeGetter rootTypeGetter)
        {
            _rootTypeDictionary = new SuffixedNamedTypeDictionary("Root");

            var validator = new DerivedTypeValidator(typeof(Root));
            var populator = new TypeDictionaryPopulator(_rootTypeDictionary, validator);

            populator.AddValidTypes(rootTypeGetter);
        }

        public DerivedRootStartInfoFactory(NamedTypeDictionary rootTypeDictionary)
        {
            _rootTypeDictionary = rootTypeDictionary;
        }

        public IEnumerable<Type> GetStemTypes(IStemsCoreServices services)
        {
            var autoActivator = new AutoActivator(services.DependencyResolver);

            return _rootTypeDictionary.GetAllTypes().Select(rootType =>
            {
                var root = (Root) autoActivator.CreateInstance(rootType);
                return root.StartStemType;
            });
        }

        public IRootStartInfo Get(IStemsCoreServices stemsServices, string startResourceName)
        {
            var autoActivator = new AutoActivator(stemsServices.DependencyResolver);
            
            var rootType = _rootTypeDictionary.GetType(startResourceName);
            var root = (Root)autoActivator.CreateInstance(rootType);

            return new DerivedRootStartInfo(root);
        }

        public RestDirectoryInfo CreateDirectoryInfo()
        {
            return _rootTypeDictionary.CreateDirectoryInfo();
        }
    }
}