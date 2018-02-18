using System;

namespace Firestorm.Client
{
    public abstract class RestClientException : Exception
    {
        internal RestClientException()
        {
            
        }
    }

    public class StatusBodyMismatchException : RestClientException
    {
    }

    public class UnknownSuccessStatusCodeException : RestClientException
    {
    }
}