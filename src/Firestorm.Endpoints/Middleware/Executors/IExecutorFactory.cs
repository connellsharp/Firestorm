namespace Firestorm.Endpoints
{
    public interface IExecutorFactory
    {
        IExecutor GetExecutor(IRestEndpoint endpoint);
    }
}