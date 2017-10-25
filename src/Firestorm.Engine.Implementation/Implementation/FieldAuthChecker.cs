using System.Collections.Generic;
using Firestorm.Engine.Fields;

namespace Firestorm.Engine
{
    internal class FieldAuthChecker<TItem>
        where TItem : class
    {
        private readonly IFieldProvider<TItem> _fieldProvider;
        private readonly IAuthorizationChecker<TItem> _authChecker;
        private readonly IDeferredItem<TItem> _item;

        public FieldAuthChecker(IFieldProvider<TItem> fieldProvider, IAuthorizationChecker<TItem> authChecker, IDeferredItem<TItem> item)
        {
            _fieldProvider = fieldProvider;
            _authChecker = authChecker;
            _item = item;
        }

        public IEnumerable<string> GetOrEnsureFields(IEnumerable<string> requestedFields, int nestedBy)
        {
            if (requestedFields == null)
                return GetDefaultAllowedFields(nestedBy);

            EnsureCanGetAllFieldsAllowed(requestedFields);
            return requestedFields;
        }

        private IEnumerable<string> GetDefaultAllowedFields(int nestedBy)
        {
            foreach (string fieldName in _fieldProvider.GetDefaultNames(nestedBy))
            {
                var field = new NamedField<TItem>(fieldName, _fieldProvider);

                if (_authChecker.CanGetField(_item, field))
                    yield return fieldName;
            }
        }

        private void EnsureCanGetAllFieldsAllowed(IEnumerable<string> fields)
        {
            foreach (string fieldName in fields)
            {
                var field = new NamedField<TItem>(fieldName, _fieldProvider);

                if (!_authChecker.CanGetField(_item, field))
                    throw new NotAuthorizedForFieldException(AuthorizableVerb.Get, fieldName);
            }
        }
    }
}