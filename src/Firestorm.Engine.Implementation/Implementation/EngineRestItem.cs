using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine
{
    public class EngineRestItem<TItem> : IRestItem
        where TItem : class
    {
        private readonly IEngineContext<TItem> _context;
        private readonly IDeferredItem<TItem> _item;

        public EngineRestItem([NotNull] IEngineContext<TItem> context, [NotNull] IDeferredItem<TItem> item)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (item == null) throw new ArgumentNullException(nameof(item));

            _context = context;
            _item = item;
        }

        public IRestResource GetField(string fieldName)
        {
            if (!_context.Fields.FieldExists(fieldName))
                throw new FieldNotFoundException(fieldName, true);
            
            var namedField = new NamedField<TItem>(fieldName, _context.Fields);

            if (!_context.AuthorizationChecker.CanGetField(_item, namedField))
                throw new NotAuthorizedForFieldException(AuthorizableVerb.Get, fieldName);

            IRestResource fullResource = _context.Fields.GetFullResource(fieldName, _item, _context.Transaction);
            if (fullResource != null)
                return fullResource;
            
            if (ScalarFieldHelper.IsScalarType(namedField.Reader.FieldType))
                return new EngineRestScalar<TItem>(_context, _item, namedField);
            
            return new EngineRestPoco<TItem>(_context, _item, namedField);
        }

        public async Task<Acknowledgment> EditAsync(RestItemData itemData)
        {
            if (!_context.AuthorizationChecker.CanEditItem(_item))
                throw new NotAuthorizedForItemException(AuthorizableVerb.Edit);

            var setter = new QueryableFieldSetter<TItem>(_context);
            await setter.SetMappedValuesAsync(_item, itemData);

            await _context.Transaction.SaveChangesAsync();
            
            if(_item.WasCreated)
                return new CreatedItemAcknowledgment(_item.Identifier);

            return new Acknowledgment();
        }

        public async Task<Acknowledgment> DeleteAsync()
        {
            if (!_context.AuthorizationChecker.CanDeleteItem(_item))
                throw new NotAuthorizedForItemException(AuthorizableVerb.Delete);

            await _item.LoadAsync();
            _context.Repository.MarkDeleted(_item.LoadedItem);
            await _context.Transaction.SaveChangesAsync();

            return new Acknowledgment();
        }

        public async Task<RestItemData> GetDataAsync(IEnumerable<string> fieldNames)
        {
            var fieldAuth = new FieldAuthChecker<TItem>(_context.Fields, _context.AuthorizationChecker, _item);
            IEnumerable<string> fields = fieldAuth.GetOrEnsureFields(fieldNames, 0);

            var selector = new QueryableFieldSelector<TItem>(_context.Fields, fields);
            var loadedObjects = await selector.SelectFieldsOnlyAsync(_item.Query, _context.Repository.ForEachAsync);
            RestItemData loadedObject = ItemQueryHelper.SingleOrThrow(loadedObjects);
            return loadedObject;
        }

    }
}