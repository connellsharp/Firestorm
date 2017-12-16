using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;
using Firestorm.Endpoints.Preconditions;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Start;
using Firestorm.Endpoints.WebApi2.ErrorHandling;

namespace Firestorm.Endpoints.WebApi2
{
    /// <summary>
    /// The Firestorm Web API Controller for ASP.NET Web API 2.0.
    /// </summary>
    /// <remarks>
    /// This has evolved and uses some middlewarey-type stuff that's designed more around OWIN and ASP.NET Core.
    /// Likely, this will be obsolete soon.
    /// </remarks>
    [RestApiExceptionFilter]
    public class FirestormController : ApiController
    {
        [Obsolete("Static config?")] // TODO: can this be done in the routing somehow?
        internal static FirestormConfiguration GlobalConfig { get; set; }

        public FirestormController()
            : this(GlobalConfig)
        { }

        private FirestormController(FirestormConfiguration configuration)
        {
            Config = configuration;
            ResponseBuilder = new AggregateResponseBuilder(new DefaultResponseBuilders(Config.EndpointConfiguration.ResponseConfiguration));
        }

        internal FirestormConfiguration Config { get; }

        internal IResponseBuilder ResponseBuilder { get; }

        [HttpGet]
        public async Task<object> GetAsync()
        {
            IRestEndpoint endpoint = GetEndpoint();

            if (!endpoint.EvaluatePreconditions(GetPreconditions()))
                return StatusCode(HttpStatusCode.NotModified);

            ResourceBody resourceBody = await endpoint.GetAsync();

            var response = new Response(ResourcePath);
            ResponseBuilder.AddResource(response, resourceBody);
            return response.Body; // TODO headers?
        }

        [HttpOptions]
        public async Task<object> OptionsAsync()
        {
            Options options = await GetEndpoint().OptionsAsync();

            var response = new Response(ResourcePath);
            ResponseBuilder.AddOptions(response, options);
            return response.Body; // TODO headers?
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostAsync([FromBody] ResourceBody body)
        {
            return await GetResultFromMethodFeedbackAsync(UnsafeMethod.Post, body);
        }

        [HttpPut]
        public async Task<IHttpActionResult> PutAsync([FromBody] ResourceBody body)
        {
            return await GetResultFromMethodFeedbackAsync(UnsafeMethod.Put, body);
        }

        [HttpPatch]
        public async Task<IHttpActionResult> PatchAsync([FromBody] ResourceBody body)
        {
            return await GetResultFromMethodFeedbackAsync(UnsafeMethod.Patch, body);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync()
        {
            return await GetResultFromMethodFeedbackAsync(UnsafeMethod.Delete, null); // TODO DELETE bodies?
        }

        private async Task<IHttpActionResult> GetResultFromMethodFeedbackAsync(UnsafeMethod method, ResourceBody body)
        {
            IRestEndpoint endpoint = GetEndpoint();
            
            if (!endpoint.EvaluatePreconditions(GetPreconditions()))
                return StatusCode(HttpStatusCode.PreconditionFailed);

            Feedback feedback = await endpoint.UnsafeAsync(method, body);
            return GetResultFromFeedback(feedback);
        }

        private IPreconditions GetPreconditions()
        {
            return new WebApiPreconditions(Request.Headers);
        }

        private IHttpActionResult GetResultFromFeedback(Feedback feedback)
        {
            var response = new Response(ResourcePath);
            ResponseBuilder.AddFeedback(response, feedback);

            object responseBody = response.GetFullBody();

            if (response.StatusCode == HttpStatusCode.Created)
            {
                return Created(response.Headers["Location"], responseBody);
            }

            if (feedback.Type == FeedbackType.MultiResponse)
            {
                Debug.Assert(response.StatusCode == (HttpStatusCode) 207);
                Debug.Assert(responseBody is object[]);
                return Content(response.StatusCode, responseBody as object[]); // for content negotiation. does this even make a difference?
            }

            return Content(response.StatusCode, responseBody);
        }

        private IRestEndpoint GetEndpoint()
        {
            Context = new HttpRequestRestEndpointContext(RequestContext, Request, Config.EndpointConfiguration);
            return StartUtilities.GetEndpointFromPath(Config.StartResourceFactory, Context, ResourcePath);
        }

        protected IRestEndpointContext Context { get; private set; }

        protected internal string ResourcePath
        {
            get { return (string) ControllerContext.RouteData.Values["path"]; }
        }

        protected override void Dispose(bool disposing)
        {
            Context?.Dispose();
            base.Dispose(disposing);
        }
    }
}