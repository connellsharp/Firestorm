using System.Collections.Generic;

namespace Firestorm.Endpoints.Strategies
{
    public class UnsafeRequestStrategies<TResource> : Dictionary<UnsafeMethod, IUnsafeRequestStrategy<TResource>>
        where TResource : IRestResource
    {
        public IUnsafeRequestStrategy<TResource> GetOrThrow(UnsafeMethod method)
        {
            if (!ContainsKey(method))
                throw new MethodNotAllowedException(method, typeof(TResource));

            return this[method];
        }
    }
}