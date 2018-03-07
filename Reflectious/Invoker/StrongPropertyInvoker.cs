using System;
using System.Linq.Expressions;

namespace Firestorm
{
    public class StrongPropertyInvoker<TInstance, TProperty> : PropertyInvoker
    {
        public StrongPropertyInvoker(TInstance instance, Expression<Func<TInstance, TProperty>> propertyExpression)
            : base(instance, LambdaMemberUtilities.GetPropertyInfoFromLambda(propertyExpression))
        {
        }

        public new TProperty GetValue()
        {
            return (TProperty)base.GetValue();
        }

        public void SetValue(TProperty value)
        {
            base.SetValue(value);
        }
    }
}