using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Stems.Attributes.Analysis;

namespace Firestorm.Stems.Attributes.Basic.Resolvers
{
    public class AuthorizeAttributeResolver : FieldAttributeResolverBase
    {
        private readonly Func<IRestUser, bool> _isAuthorized;

        public AuthorizeAttributeResolver(Func<IRestUser, bool> isAuthorized)
        {
            _isAuthorized = isAuthorized;
        }

        public override void IncludeMember(MemberInfo member)
        {
            SetFieldDefinition(member);
            FieldDefinition.AuthorizePredicate = _isAuthorized;
        }

        [ExcludeFromCodeCoverage]
        protected override void AddExpressionToDefinition(LambdaExpression expression)
        {
            Debug.Fail("Shouldn't get here due to override of IncludeMember.");
            throw new StemAttributeSetupException("Cannot apply AuthorizeAttriute to this member.");
        }

        [ExcludeFromCodeCoverage]
        protected override Type AddMethodToDefinition(MethodInfo method)
        {
            Debug.Fail("Shouldn't get here due to override of IncludeMember.");
            throw new StemAttributeSetupException("Cannot apply AuthorizeAttriute to this member.");
        }
    }
}