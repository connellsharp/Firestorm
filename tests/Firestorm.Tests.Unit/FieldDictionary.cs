using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Additives.Writers;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Tests.Unit.Engine.Models
{
    /// <summary>
    /// Maps API field names to the relevent properties.
    /// </summary>
    /// <typeparam name="TItem">The type of item to map field names for.</typeparam>
    public class FieldDictionary<TItem> : Dictionary<string, Field<TItem>>, IFieldProvider<TItem>
        where TItem : class
    {
        public virtual IEnumerable<string> GetDefaultNames(int nestedBy)
        {
            if (nestedBy == 1)
                return Keys.Take(1); // TODO assuming ID is the first field
            else
                return Keys;
        }

        public bool FieldExists(string fieldName)
        {
            return ContainsKey(fieldName);
        }

        public IRestResource GetFullResource(string fieldName, IDeferredItem<TItem> item, IDataTransaction dataTransaction)
        {
            return null;
        }

        public IFieldReader<TItem> GetReader(string fieldName)
        {
            return this[fieldName].Reader;
        }

        public IFieldCollator<TItem> GetCollator(string fieldName)
        {
            return this[fieldName].Collator;
        }

        public IFieldWriter<TItem> GetWriter(string fieldName)
        {
            return this[fieldName].Writer;
        }

        public IFieldDescription GetDescription(string fieldName, CultureInfo cultureInfo)
        {
            throw new NotSupportedException("FieldDictionary implementation does not support field descriptions.");
        }

        /// <summary>
        /// Maps the field name to a property. Expression must be a simple property getter expression.
        /// </summary>
        public void Add<TProperty>(string apiFieldName, [NotNull] Expression<Func<TItem, TProperty>> propertyExpression)
        {
            var reader = new PropertyExpressionFieldReader<TItem, TProperty>(propertyExpression);
            
            Add(apiFieldName, new Field<TItem>
            {
                Reader = reader,
                Collator = new BasicFieldCollator<TItem>(reader),
                Writer = new PropertyExpressionFieldWriter<TItem, TProperty>(propertyExpression)
            });
        }
    }
}