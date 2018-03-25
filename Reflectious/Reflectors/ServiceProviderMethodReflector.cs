using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Firestorm
{
    public class ServiceProviderMethodReflector<TInstance, TReturn>: MethodReflectorBase<TInstance, TReturn>
    {
        private readonly IServiceProvider _serviceProvider;

        internal ServiceProviderMethodReflector(TInstance instance, [NotNull] IMethodFinder methodFinder, 
            IServiceProvider serviceProvider) 
            : base(instance, methodFinder)
        {
            if (methodFinder.ParameterTypes == null)
                throw new NoParameterTypesException();
            
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Invokes the method using parameter values from the <see cref="IServiceProvider"/>.
        /// </summary>
        [PublicAPI]
        public TReturn Invoke()
        {
            object[] args = MethodFinder.ParameterTypes.Select(t => _serviceProvider.GetService(t)).ToArray();
            return base.Invoke(args);
        }
    }
}