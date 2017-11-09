using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Queryable.Exceptions;
using JetBrains.Annotations;

namespace Firestorm.Engine.Queryable
{
    public class QueryableFieldSetter<TItem>
        where TItem : class
    {
        private readonly IDataTransaction _dataTransaction;
        private readonly IFieldProvider<TItem> _fieldProvider;
        private readonly IAuthorizationChecker<TItem> _authChecker;

        internal QueryableFieldSetter([NotNull] IEngineContext<TItem> context)
            : this(context.Transaction, context.Fields, context.AuthorizationChecker)
        { }

        public QueryableFieldSetter([NotNull] IDataTransaction dataTransaction, [NotNull] IFieldProvider<TItem> fieldProvider, [NotNull] IAuthorizationChecker<TItem> authChecker)
        {
            _dataTransaction = dataTransaction ?? throw new ArgumentNullException(nameof(dataTransaction));
            _fieldProvider = fieldProvider ?? throw new ArgumentNullException(nameof(fieldProvider));
            _authChecker = authChecker ?? throw new ArgumentNullException(nameof(authChecker));
        }

        public async Task SetMappedValuesAsync([NotNull] IDeferredItem<TItem> deferredItem, [NotNull] RestItemData itemData)
        {
            foreach (KeyValuePair<string, object> kvp in itemData)
            {
                if (!_fieldProvider.FieldExists(kvp.Key))
                    throw new FieldNotFoundException(kvp.Key, false);
                
                IFieldWriter<TItem> fieldWriter = _fieldProvider.GetWriter(kvp.Key);
                if(fieldWriter == null)
                    throw new FieldOperationNotAllowedException(kvp.Key, FieldOperationNotAllowedException.Operation.Write);

                var namedField = new NamedField<TItem>(kvp.Key, _fieldProvider);
                if (!_authChecker.CanEditField(deferredItem, namedField))
                    throw new NotAuthorizedForFieldException(AuthorizableVerb.Edit, kvp.Key);

                await fieldWriter.SetValueAsync(deferredItem, kvp.Value, _dataTransaction);
            }
        }
    }
}