using System;

namespace Firestorm.Endpoints
{
    public class FirestormConfigurationException : Exception
    {
        internal FirestormConfigurationException(string message)
            : base(message)
        {

        }
    }
}