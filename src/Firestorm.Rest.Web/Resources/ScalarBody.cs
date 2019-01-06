namespace Firestorm.Rest.Web
{
    public class ScalarBody : ResourceBody
    {
        public ScalarBody(object scalar)
        {
            Scalar = scalar;
        }

        public object Scalar { get; }

        public override ResourceType ResourceType { get; } = ResourceType.Scalar;

        public override object GetObject() => Scalar;
    }
}