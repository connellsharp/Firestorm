using Firestorm.Stems.Fuel.Resolving.Exceptions;

namespace Firestorm.Stems.Roots
{
    /// <summary>
    /// Exception that is thrown when Stem startup has been setup incorrectly.
    /// </summary>
    public class StemStartSetupException : StemException
    {
        public StemStartSetupException(string message)
            : base(message)
        { }
    }
}