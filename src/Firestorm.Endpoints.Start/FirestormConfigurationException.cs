using System;

namespace Firestorm.Endpoints.Start
{
    public class FirestormConfigurationException : Exception
    {
        internal FirestormConfigurationException(string message)
            : base(message)
        {

        }
    }
}