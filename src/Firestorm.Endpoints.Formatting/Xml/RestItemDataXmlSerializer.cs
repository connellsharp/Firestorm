using System.Xml.Serialization;

namespace Firestorm.Endpoints.Formatting.Xml
{
    public class RestItemDataXmlSerializer : XmlSerializer
    {
        public override bool CanDeserialize(System.Xml.XmlReader xmlReader)
        {
            return base.CanDeserialize(xmlReader);
        }
        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader)
        {
            return base.Deserialize(reader);
        }
        protected override void Serialize(object o, System.Xml.Serialization.XmlSerializationWriter writer)
        {
            base.Serialize(o, writer);
        }
    }
}
