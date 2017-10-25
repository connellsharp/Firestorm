using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Firestorm.Endpoints.Formatting.Json
{
    /// <summary>
    /// Converts the JToken to either a type of <see cref="TObject"/>, <see cref="IEnumerable{TObject}"/> or a scalar type.
    /// </summary>
    internal class JObjectConverter<TObject>
        where TObject : IDictionary<string, object>, new()
    {
        /// <summary>
        /// Converts the JToken to either a type of <see cref="TObject"/>, <see cref="IEnumerable{TObject}"/> or a scalar type.
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
        /// Adds the properties of the given <see cref="jObject" /> to a new instance of the <see cref="TObject"/> dictionary type.
        /// </summary>
        internal TObject ConvertObject(JObject jObject)
        {
            var obj = new TObject();
            IDictionary<string, object> dictionary = obj;

            foreach (JProperty property in jObject.Properties())
            {
                dictionary.Add(property.Name, Convert(property.Value));
            }

            return obj;
        }
    }
}