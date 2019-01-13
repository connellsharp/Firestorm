using System;
using System.Net;
using System.Threading;

namespace Firestorm.Testing.Http
{
    public static class Attempt
    {
        public static T KeepTrying<T>(Func<T> func, int[] waitTimes)
        {
            for (int i = 0; i < waitTimes.Length + 1; i++)
            {
                try
                {
                    return func();
                }
                catch (HttpListenerException) when (i < waitTimes.Length)
                {
                    Thread.Sleep(waitTimes[i]);
                }
            }
            
            throw new InvalidOperationException("Exception thrown in KeepTrying that wasn't rethrown.");
        }
    }
}