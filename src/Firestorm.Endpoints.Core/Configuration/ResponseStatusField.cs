namespace Firestorm.Endpoints.Responses
{
    /// <summary>
    /// Behaviour of an additional field added to response bodies to indicate status.
    /// </summary>
    public enum ResponseStatusField
    {
        /// <summary>
        /// No extra status field should be added to the response.
        /// Normal acknowledgements should be empty responses.
        /// </summary>
        None,

        /// <summary>
        /// A descriptive status code field e.g. { "status": "ok" }.
        /// </summary>
        StatusCode,

        /// <summary>
        /// A boolean sucess flag e.g. { "success": true }.
        /// </summary>
        SuccessBoolean
    }
}