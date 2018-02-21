using Firestorm.Stems.Roots.Combined;

namespace Firestorm.Stems.Roots.Derive
{
    /// <summary>
    /// Contains the user configuration for Stems and creates starting collections from <see cref="Root"/> implementations.
    /// </summary>
    public class DerivedRootsResourceFactory : RootResourceFactoryBase
    {
        public ITypeGetter RootTypeGetter { get; set; }

        protected override IRootStartInfoFactory CreateStartInfoFactory()
        {
            return new DerivedRootStartInfoFactory(RootTypeGetter);
        }
    }
}