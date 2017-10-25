using System;

namespace Firestorm.Stems.Fuel.Resolving.Exceptions
{
    public class StemException : Exception
    {
        protected StemException(string message)
            : base(message)
        { }
    }
}