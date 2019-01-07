using System;

namespace Firestorm.Stems.Analysis
{
    /// <summary>
    /// Exception that is thrown when <see cref="StemAttribute"/>s have been setup incorrectly.
    /// </summary>
    public class StemAttributeSetupException : Exception // TODO StemException from Power?
    {
        public StemAttributeSetupException(string message)
            : base(message)
        { }

        public StemAttributeSetupException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}