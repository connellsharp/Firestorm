using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine
{
    /// <summary>
    /// Represents a non-engine item that has been returned as if it were a scalar value.
    /// </summary>
    /// <remarks>
    /// TODO shouldn't actually be scalar, but this is just copied.
    /// </remarks>
    public class EngineRestPoco<TItem> : IRestScalar 
        where TItem : class
    {
        private readonly IEngineContext<TItem> _context;
        private readonly IDeferredItem<TItem> _item;
        private readonly INamedField<TItem> _field;

        internal EngineRestPoco([NotNull] IEngineContext<TItem> context, [NotNull] IDeferredItem<TItem> item, [NotNull] INamedField<TItem> field)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (field == null) throw new ArgumentNullException(nameof(field));

            _context = context;
            _item = item;
            _field = field;
        }

        public async Task<object> GetAsync()
        {
            if (!_context.AuthorizationChecker.CanGetField(_item, _field))
                throw new NotAuthorizedForFieldException(AuthorizableVerb.Get, _field.Name);

            IFieldReader<TItem> value = _field.Reader;

            return await ScalarFieldHelper.LoadScalarValueAsync(value, _item.Query, _context.Repository.ForEachAsync);
        }

        public async Task<Acknowledgment> EditAsync(object value)
        {
            if (!_context.AuthorizationChecker.CanEditField(_item, _field))
                throw new NotAuthorizedForFieldException(AuthorizableVerb.Edit, _field.Name);

            await _item.LoadAsync(); // TODO: is this necessary?
            await _field.Writer.SetValueAsync(_item, value, _context.Transaction);
            await _context.Transaction.SaveChangesAsync();

            return new Acknowledgment();
        }
    }
}