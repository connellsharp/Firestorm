using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Firestorm
{
    /// <summary>
    /// Core class containing fields and values of an item used in a Firestorm API.
    /// </summary>
    public class RestItemData : Dictionary<string, object>
    {
        /// <summary>
        /// The list of importers used to import objects when passed into the constructor.
        /// </summary>
        public static IList<IRestItemDataImporter> Importers { get; } = new List<IRestItemDataImporter>
        {
            new RestItemDataImporter(),
            new DictionaryImporter(),
            new CustomTypeImporter(),
            new DataContractImporter(),
            new AllObjectImporter()
        };

        public RestItemData()
        {
        }

        public RestItemData([NotNull] IDictionary<string, object> dictionary)
            : base(dictionary)
        {
        }

        public RestItemData([NotNull] object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            ImportObject(obj);
        }

        public Type Type { get; private set; }

        private void ImportObject(object obj)
        {
            foreach (IRestItemDataImporter importer in Importers)
            {
                if (!importer.CanImport(obj))
                    continue;

                Type = importer.GetType(obj);

                foreach (KeyValuePair<string, object> pair in importer.GetValues(obj))
                {
                    Add(pair.Key, pair.Value);
                }

                return;
            }
        }
    }
}