using System;
using Firestorm.Engine.Identifiers;

namespace Firestorm.Engine.Defaults
{
    public class SingleIdentifierProvider<TItem> : IIdentifierProvider<TItem>
    {
        private readonly IIdentifierInfo<TItem> _identifierInfo;

        public SingleIdentifierProvider(IIdentifierInfo<TItem> identifierInfo)
        {
            _identifierInfo = identifierInfo;
        }

        public IIdentifierInfo<TItem> GetInfo(string identifierName)
        {
            if (identifierName == null)
                return _identifierInfo;

            throw new NotSupportedException("This collection does not support named identifiers.");
        }
    }
}