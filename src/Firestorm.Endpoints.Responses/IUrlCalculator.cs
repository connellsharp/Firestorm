namespace Firestorm.Endpoints.Responses
{
    internal interface IUrlCalculator
    {
        /// <summary>
        /// Works out the full URL for the next page based on the given <see cref="pageInstruction"/>.
        /// Used in the Link header and page links on the response body.
        /// </summary>
        string GetPageUrl(PageInstruction pageInstruction);

        /// <summary>
        /// Works out the URL for a new item that has just been created.
        /// Used in the Location header with a 201 Created response.
        /// </summary>
        string GetCreatedUrl(object newIdentifier);
    }
}