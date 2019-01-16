using System.Collections.Generic;

namespace Firestorm.Endpoints.Requests
{
    public class CommandStrategies<TResource> : Dictionary<UnsafeMethod, ICommandStrategy<TResource>>
        where TResource : IRestResource
    {
        public ICommandStrategy<TResource> GetOrThrow(UnsafeMethod method)
        {
            if (!ContainsKey(method))
                throw new MethodNotAllowedException(method, typeof(TResource));

            return this[method];
        }
    }
}