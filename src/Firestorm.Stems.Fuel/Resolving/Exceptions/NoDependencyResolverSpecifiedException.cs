using System;

namespace Firestorm.Stems.Fuel.Resolving.Exceptions
{
    internal class NoDependencyResolverSpecifiedException : StemException
    {
        public NoDependencyResolverSpecifiedException(Type stemType)
            : base("Stem type '" + stemType.Namespace + "' has constructor parameters but no dependency resolver was specified..")
        {
        }
    }
}