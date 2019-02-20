namespace Firestorm.Endpoints.Executors
{
    public interface IExecutorFactory
    {
        IExecutor GetExecutor(IRestEndpoint endpoint);
    }
}