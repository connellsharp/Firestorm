using System;

namespace Firestorm.Endpoints.Web
{
    public class FirestormConfigurationException : Exception
    {
        internal FirestormConfigurationException(string message)
            : base(message)
        {

        }
    }
}