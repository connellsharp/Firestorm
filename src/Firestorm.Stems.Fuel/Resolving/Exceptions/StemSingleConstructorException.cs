using System;

namespace Firestorm.Stems.Fuel.Resolving.Exceptions
{
    internal class StemSingleConstructorException : StemException
    {
        public StemSingleConstructorException(Type stemType)
            : base("Stem types should only have one constructor, but type '" + stemType.Namespace + "' has zero or multiple.")
        {
        }
    }
}