namespace Firestorm
{
    public static class InvokerExtension
    {   
        public static Invoker<T> Invoker<T>(this T instance)
        {
            return new Invoker<T>(instance);
        }
    }
}