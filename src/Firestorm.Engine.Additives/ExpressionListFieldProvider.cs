using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using Firestorm.Data;
using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Additives.Writers;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Fields;

namespace Firestorm.Engine.Additives
{
    public class ExpressionListFieldProvider<TItem> : IFieldProvider<TItem>, IEnumerable<LambdaExpression>
        where TItem : class
    {
        readonly Dictionary<string, IFieldReader<TItem>> _readers = new Dictionary<string, IFieldReader<TItem>>();
        readonly Dictionary<string, IFieldCollator<TItem>> _collators = new Dictionary<string, IFieldCollator<TItem>>();
        readonly Dictionary<string, IFieldWriter<TItem>> _writers = new Dictionary<string, IFieldWriter<TItem>>();
        private readonly List<LambdaExpression> _expressions = new List<LambdaExpression>();

        public void Add<TValue>(string name, Expression<Func<TItem, TValue>> getValue)
        {
            _expressions.Add(getValue);
            
            var reader = new ExpressionFieldReader<TItem, TValue>(getValue);
            
            _readers.Add(name, reader);
            _collators.Add(name, new BasicFieldCollator<TItem>(reader));
            _writers.Add(name, new PropertyExpressionFieldWriter<TItem, TValue>(getValue));
        }

        public IEnumerable<string> GetDefaultNames(int nestedBy)
        {
            return _readers.Keys;
        }

        public bool FieldExists(string fieldName)
        {
            return _readers.ContainsKey(fieldName);
        }

        public IRestResource GetFullResource(string fieldName, IDeferredItem<TItem> item, IDataTransaction dataTransaction)
        {
            return null;
        }

        public IFieldReader<TItem> GetReader(string fieldName)
        {
            return _readers[fieldName];
        }

        public IFieldCollator<TItem> GetCollator(string fieldName)
        {
            return _collators[fieldName];
        }

        public IFieldWriter<TItem> GetWriter(string fieldName)
        {
            return _writers[fieldName];
        }

        public IFieldDescription GetDescription(string fieldName, CultureInfo cultureInfo)
        {
            throw new NotImplementedException("Not implemented descriptions in the ExpressionListFieldProvider");
        }

        public IEnumerator<LambdaExpression> GetEnumerator()
        {
            return _expressions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}