using System;
using System.Collections.Generic;
using System.Linq;
using Firestorm.Endpoints.Naming;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace Firestorm.Endpoints.Formatting.Json
{
    /// <summary>
    /// Converts the JToken to either a type of <see cref="TDictionary"/>, <see cref="IEnumerable{TDictionary}"/> or a scalar type.
    /// </summary>
    internal class JObjectDictionaryCreator<TDictionary>
        where TDictionary : IDictionary<string, object>, new()
    {
        private readonly INamingConventionSwitcher _switcher;

        public JObjectDictionaryCreator([CanBeNull] INamingConventionSwitcher switcher)
        {
            _switcher = switcher ?? new VoidNamingConventionSwitcher();
        }

        /// <summary>
        /// Converts the JToken to either a type of <see cref="TDictionary"/>, <see cref="IEnumerable{TDictionary}"/> or a scalar type.
        /// </summary>
        internal object Convert(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Array:
                    return token.Select(Convert).ToArray();

                case JTokenType.Object:
                    return ConvertObject((JObject)token);

                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Boolean:
                case JTokenType.Integer:
                case JTokenType.Date:
                case JTokenType.Bytes:
                case JTokenType.Guid:
                case JTokenType.Uri:
                case JTokenType.TimeSpan:
                    return token.ToObject<object>();

                case JTokenType.None:
                case JTokenType.Constructor:
                case JTokenType.Property:
                case JTokenType.Comment:
                case JTokenType.Raw:
                    throw new NotSupportedException("Firestorm cannot convert this type of JToken.");

                case JTokenType.Null:
                case JTokenType.Undefined:
                    return null;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Adds the properties of the given <see cref="jObject" /> to a new instance of the <see cref="TDictionary"/> dictionary type.
        /// </summary>
        internal TDictionary ConvertObject(JObject jObject)
        {
            var obj = new TDictionary();
            IDictionary<string, object> dictionary = obj;

            foreach (JProperty property in jObject.Properties())
            {
                string key = _switcher.ConvertRequestedToCoded(property.Name);
                dictionary.Add(key, Convert(property.Value));
            }

            return obj;
        }
    }
}