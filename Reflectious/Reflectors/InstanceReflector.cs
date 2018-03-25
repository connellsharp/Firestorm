using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Firestorm
{
   
    public class InstanceReflector<TInstance> : InstanceReflector
    {
        internal InstanceReflector(IInstanceGetter instanceGetter)
            : base(instanceGetter)
        {
        }

        public WeakMethodReflector<TInstance, TReturn> GetMethod<TReturn>(Expression<Func<TInstance, TReturn>> expression)
        {
            TInstance instance = (TInstance)InstanceGetter.GetInstance();
            var finder = new ExpressionMethodFinder<TInstance, TReturn>(expression);
            return new WeakMethodReflector<TInstance, TReturn>(instance, finder);
        }

        public PropertyReflector<TInstance, TProperty> GetProperty<TProperty>(Expression<Func<TInstance, TProperty>> propertyExpression)
        {
            TInstance instance = (TInstance)InstanceGetter.GetInstance();
            var finder = new ExpressionPropertyFinder<TInstance, TProperty>(propertyExpression);
            return new PropertyReflector<TInstance, TProperty>(instance, finder);
        }
    }

    public class InstanceReflector
    {
        internal readonly IInstanceGetter InstanceGetter;

        internal InstanceReflector([NotNull] IInstanceGetter instanceGetter)
        {
            InstanceGetter = instanceGetter;
        }

        public WeakMethodReflector GetMethod(string methodName, Assume assume = Assume.Nothing)
        {
            object instance = InstanceGetter.GetInstance();
            var finder = FinderUtility.GetMethodFinder(InstanceGetter.Type, methodName, instance == null, assume);
            return new WeakMethodReflector(instance, finder);
        }

        public WeakMethodReflector GetExtensionMethod(Type extenstionsClassType, string methodName, Assume assume = Assume.Nothing)
        {
            object instance = InstanceGetter.GetInstance();
            var finder = FinderUtility.GetMethodFinder(extenstionsClassType, methodName, true, assume);
            finder = FinderUtility.WrapForExtension(finder, InstanceGetter.Type);
            return new WeakMethodReflector(instance, finder);
        }

        public PropertyReflector GetProperty(string propertyName)
        {
            object instance = InstanceGetter.GetInstance();
            var finder = new PropertyFinder(InstanceGetter.Type, propertyName, instance == null);
            return new PropertyReflector(instance, finder);
        }
    }

    public class WeakExtensionMethodReflector : WeakMethodReflector
    {
        internal WeakExtensionMethodReflector(object instance, IMethodFinder finder)
            : base(instance, finder)
        {
            
        }
    }
}