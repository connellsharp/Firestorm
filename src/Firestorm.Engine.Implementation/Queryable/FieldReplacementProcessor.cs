using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Firestorm.Engine.Fields;
using JetBrains.Annotations;

namespace Firestorm.Engine.Queryable
{
    public class FieldReplacementProcessor<TItem>
    {
        private readonly Func<IEnumerable<KeyValuePair<string, IFieldReader<TItem>>>> _getFieldReadersFunc;
        private ConcurrentDictionary<string, IFieldValueReplacer<TItem>> _replacerDictionary;

        public FieldReplacementProcessor(IDictionary<string, IFieldReader<TItem>> fieldReaders)
        {
            _getFieldReadersFunc = () => fieldReaders;
        }

        public FieldReplacementProcessor(Func<IEnumerable<KeyValuePair<string, IFieldReader<TItem>>>> getFieldReadersFunc)
        {
            _getFieldReadersFunc = getFieldReadersFunc;
        }

        public Task LoadAllAsync(IQueryable<TItem> items)
        {
            _replacerDictionary = new ConcurrentDictionary<string, IFieldValueReplacer<TItem>>();
            var tasks = new List<Task>();
            var fieldReaders = _getFieldReadersFunc.Invoke();

            foreach (KeyValuePair<string, IFieldReader<TItem>> fieldReader in fieldReaders)
            {
                IFieldValueReplacer<TItem> replacer = fieldReader.Value.Replacer;
                if (replacer == null)
                    continue;

                Task preloadTask = replacer.LoadAsync(items).ContinueWith(task =>
                {
                    if (!_replacerDictionary.TryAdd(fieldReader.Key, replacer))
                        throw new InvalidOperationException("Error adding reader replacer to dictionary.");
                });

                tasks.Add(preloadTask);
            }

            return Task.WhenAll(tasks);
        }

        public void Replace([NotNull] object dynamicObj, Type dynamicType)
        {
            Debug.Assert(dynamicObj != null, "dynamicObj should not be null.");

            foreach (var replacer in _replacerDictionary)
            {
                Debug.Assert(replacer.Value != null, "Field value should not be in the preloaded list if there is no replacer.");

                FieldInfo fieldInfo = dynamicType.GetField(replacer.Key);

                object dbValue = fieldInfo.GetValue(dynamicObj);
                object replacementValue = replacer.Value.GetReplacement(dbValue);

                fieldInfo.SetValue(dynamicObj, replacementValue);
            }
        }
    }
}