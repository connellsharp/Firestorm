using System;

namespace Firestorm.Stems
{
    public class DefaultDependencyResolver : IDependencyResolver
    {
        private readonly Func<Type, object> _resolveFunc;

        public DefaultDependencyResolver(IServiceProvider serviceProvider)
        {
            _resolveFunc = serviceProvider.GetService;
        }

        public DefaultDependencyResolver(Func<Type, object> resolveFunc)
        {
            _resolveFunc = resolveFunc;
        }

        public object Resolve(Type type)
        {
            return _resolveFunc(type);
        }
    }
}