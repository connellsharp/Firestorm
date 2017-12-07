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
        }

        internal FirestormConfiguration Config { get; }

        [HttpGet]
        public async Task<object> GetAsync()
        {
            IRestEndpoint endpoint = GetEndpoint();

            if (!endpoint.EvaluatePreconditions(GetPreconditions()))
                return StatusCode(HttpStatusCode.NotModified);

            ResourceBody resourceBody = await endpoint.GetAsync();

            // TODO support pagination via LinkHeaderBuilder ?
            //if (resourceBody is IPagedResourceBody pagedResourceBody)
            //{
            //    var builder = new LinkHeaderBuilder();
            //    builder.AddDetails(pagedResourceBody.PageDetails);
            //    builder.GetHeaderValue();
            //}

            return Config.EndpointConfiguration.ResponseContentGenerator.GetFromResource(resourceBody);
        }

        [HttpOptions]
        public async Task<object> OptionsAsync()
        {
            Options options = await GetEndpoint().OptionsAsync();
            return Config.EndpointConfiguration.ResponseContentGenerator.GetFromOptions(options);
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
            var responseBuilder = new AggregateResponseBuilder(new DefaultResponseBuilders(Config.EndpointConfiguration));
            responseBuilder.AddFeedback(response, feedback);

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

        protected string ResourcePath
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