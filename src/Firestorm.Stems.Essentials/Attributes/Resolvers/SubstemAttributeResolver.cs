using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Firestorm.Stems.Attributes.Basic.Resolvers
{
    public class SubstemAttributeResolver : FieldAttributeResolverBase
    {
        private readonly Type _substemType;

        public SubstemAttributeResolver(Type substemType)
        {
            _substemType = substemType;
        }

        protected override void AddExpressionToDefinition(LambdaExpression expression)
        {
            FieldDefinition.SubstemType = _substemType;
        }

        protected override Type AddMethodToDefinition(MethodInfo method)
        {
            throw new InvalidOperationException("Substem attribute cannot be applied to methods.");
        }
    }
}