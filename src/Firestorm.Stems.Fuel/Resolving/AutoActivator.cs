using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Firestorm.Stems.Fuel.Resolving.Exceptions;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel.Resolving
{
    /// <summary>
    /// Contains methosd to construct instances of objects using automatically resolved constructor arguments.
    /// </summary>
    public sealed class AutoActivator
    {
        private readonly IDependencyResolver _dependencyResolver;

        public AutoActivator(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public T CreateInstance<T>()
        {
            return (T) CreateInstance(typeof(T));
        }

        public object CreateInstance([NotNull] Type type)
        {
            object[] constructorArgs = GetConstructorArguments(type).ToArray();

            return Activator.CreateInstance(type, constructorArgs);
        }

        private IEnumerable<object> GetConstructorArguments(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();

            if (constructors.Length != 1)
                throw new StemSingleConstructorException(type);

            ParameterInfo[] parameters = constructors[0].GetParameters();

            if (parameters.Length == 0)
                yield break;

            if (_dependencyResolver == null)
                throw new NoDependencyResolverSpecifiedException(type);

            foreach (ParameterInfo paramInfo in parameters)
            {
                yield return _dependencyResolver.Resolve(paramInfo.ParameterType);
            }
        }
    }
}