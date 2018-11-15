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
using Firestorm.AspNetWebApi2.ErrorHandling;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Formatting.Naming;
using Firestorm.Endpoints.Query;
using Firestorm.Endpoints.Web;
using Firestorm.Host;

namespace Firestorm.AspNetWebApi2
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
            _config = configuration;
        }

        private readonly FirestormConfiguration _config;
        private Response _response;
        private ResponseBuilder _responseBuilder;

        internal Response Response
        {
            get
            {
                if (_response == null) Load();
                return _response;
            }
        }

        internal ResponseBuilder ResponseBuilder
        {
            get
            {
                if (_responseBuilder == null) Load();
                return _responseBuilder;
            }
        }

        private void Load()
        {
            var modifiers = new DefaultResponseModifiers(_config.EndpointConfiguration.ResponseConfiguration);
            _response = new Response(ResourcePath);
            _responseBuilder = new ResponseBuilder(Response, modifiers);
        }

        [HttpGet]
        public async Task<object> GetAsync()
        {
            IRestEndpoint endpoint = GetEndpoint();

            if (!endpoint.EvaluatePreconditions(GetPreconditions()))
                return StatusCode(HttpStatusCode.NotModified);

            ResourceBody resourceBody = await endpoint.GetAsync(GetQuery());

            ResponseBuilder.AddResource(resourceBody);
            return Response.ResourceBody; // TODO headers?
        }

        private IRestCollectionQuery GetQuery()
        {
            var query = new QueryStringCollectionQuery(_config.EndpointConfiguration.QueryStringConfiguration, Request.RequestUri.Query);
            return NameSwitcherUtility.TryWrapQuery(query, _config.EndpointConfiguration.NamingConventionSwitcher);
        }

        [HttpOptions]
        public async Task<object> OptionsAsync()
        {
            Options options = await GetEndpoint().OptionsAsync();

            ResponseBuilder.AddOptions(options);
            return Response.ResourceBody; // TODO headers?
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
            ResponseBuilder.AddFeedback(feedback);

            object responseBody = Response.GetFullBody();

            if (Response.StatusCode == HttpStatusCode.Created)
            {
                return Created(Response.Headers["Location"], responseBody);
            }

            if (feedback.Type == FeedbackType.MultiResponse)
            {
                Debug.Assert(Response.StatusCode == (HttpStatusCode) 207);
                Debug.Assert(responseBody is object[]);
                return Content(Response.StatusCode, responseBody as object[]); // for content negotiation. does this even make a difference?
            }

            return Content(Response.StatusCode, responseBody);
        }

        private string ResourcePath
        {
            get { return (string) ControllerContext.RouteData.Values["path"]; }
        }

        private IRequestContext _context;

        private IRestEndpoint GetEndpoint()
        {
            _context = new WebApiRequestContext(RequestContext);
            var navigator = new EndpointNavigator(_context, _config.StartResourceFactory, _config.EndpointConfiguration);
            return navigator.GetEndpointFromPath(ResourcePath);
        }

        protected override void Dispose(bool disposing)
        {
            _context?.Dispose();
            base.Dispose(disposing);
        }
    }
}