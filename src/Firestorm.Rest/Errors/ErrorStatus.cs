namespace Firestorm
{
    /// <summary>
    /// The status given for an error. Thrown in a <see cref="RestApiException"/>.
    /// </summary>
    public enum ErrorStatus
    {
        //OK = 200,
        //Created = 201,
        //NoContent = 204,
        //MultiStatus = 207,
        MovedPermanently = 301,
        Redirect = 302,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        PreconditionFailed = 412,
        InternalServerError = 500,
        NotImplemented = 501
    }
}