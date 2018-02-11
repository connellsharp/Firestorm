using System.Collections.Generic;
using Firestorm.Core;

namespace Firestorm.Endpoints.Strategies
{
    public class UnsafeRequestStrategies<TResource> : Dictionary<UnsafeMethod, IUnsafeRequestStrategy<TResource>>
        where TResource : IRestResource
    {
        public IUnsafeRequestStrategy<TResource> GetOrThrow(UnsafeMethod method)
        {
            if (!ContainsKey(method))
            {
                string resourceType = typeof(TResource).Name.Replace("IRest", string.Empty).ToLower();
                throw new MethodNotAllowedException("The " + method.ToString().ToUpper() + " method is not allowed on a " + resourceType);
            }

            return this[method];
        }
    }
}