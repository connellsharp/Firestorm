using System.Runtime.Serialization;

namespace Firestorm.Client
{
    // TODO: should this be shared with the server-side code?
    [DataContract]
    public class SuccessOrFailModel
    {
        public SuccessOrFailModel()
        {
            Success = true;
        }

        [DataMember(Name = "success")]
        public bool Success { get; protected set; }

        [DataMember(Name = "identifier")]
        public object NewIdentifier { get; private set; }

        [DataMember(Name = "error")]
        public string ErrorCode { get; private set; }

        [DataMember(Name = "error_description")]
        public string ErrorDescription { get; private set; }
    }
}