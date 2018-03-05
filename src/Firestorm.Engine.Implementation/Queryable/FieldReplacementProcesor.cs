using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Fields;

namespace Firestorm.Engine.Queryable
{
    public class FieldReplacementProcesor<TItem>
    {
        private readonly IDictionary<string, IFieldReader<TItem>> _fieldReaders;
        private ConcurrentDictionary<string, IFieldValueReplacer<TItem>> _replacerDictionary;

        public FieldReplacementProcesor(IDictionary<string, IFieldReader<TItem>> fieldReaders)
        {
            _fieldReaders = fieldReaders;
        }

        public Task LoadAllAsync(IQueryable<TItem> items)
        {
            _replacerDictionary = new ConcurrentDictionary<string, IFieldValueReplacer<TItem>>();
            var tasks = new List<Task>();

            foreach (KeyValuePair<string, IFieldReader<TItem>> fieldReader in _fieldReaders)
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

        public void Replace(object dynamicObj, Type dynamicType)
        {
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