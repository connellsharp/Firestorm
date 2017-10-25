using System.Runtime.Serialization;

namespace Firestorm.Endpoints.WebApi.ErrorHandling
{
    // TODO: should these be shared with the client code?

    [DataContract]
    public class SuccessModel
    {
        public SuccessModel()
        {
            Success = true;
        }

        [DataMember(Name = "success", Order = 0)]
        public bool Success { get; protected set; }
    }

    [DataContract]
    public class ResourceModel : SuccessModel
    {
        public ResourceModel(object resource)
        {
            Success = true;
            Resource = resource;
        }

        [DataMember(Name = "resource", Order = 1)]
        public object Resource { get; protected set; }
    }

    [DataContract]
    public class CreatedSuccessModel : SuccessModel
    {
        public CreatedSuccessModel(object newReference)
        {
            Success = true;
            NewReference = newReference;
        }

        [DataMember(Name = "reference", Order = 1)]
        public object NewReference { get; private set; }
    }

    [DataContract]
    public class ErrorModel : SuccessModel
    {
        public ErrorModel(string errorType, string errorDescription)
        {
            Success = false;
            ErrorType = errorType;
            ErrorDescription = errorDescription;
        }

        [DataMember(Name = "error", Order = 1)]
        public string ErrorType { get; private set; }

        [DataMember(Name = "error_description", Order = 2)]
        public string ErrorDescription { get; private set; }
    }
}