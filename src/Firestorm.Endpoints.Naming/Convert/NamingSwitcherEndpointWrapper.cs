using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;
using Firestorm.Endpoints.Preconditions;

namespace Firestorm.Endpoints.Naming
{
    /// <summary>
    /// Wraps an existing endpoint and exposes the same interface, but converts naming conventions using the given <see cref="INamingConventionSwitcher"/>.
    /// </summary>
    /// <remarks>
    /// I'm not sure I really like this pattern. It's replaced two similar patterns from Stems.
    /// </remarks>
    public class NamingSwitcherEndpointWrapper : IRestEndpoint
    {
        private readonly IRestEndpoint _underlyingEndpoint;
        private readonly INamingConventionSwitcher _namingConventionSwitcher;

        public NamingSwitcherEndpointWrapper(IRestEndpoint underlyingEndpoint, INamingConventionSwitcher namingConventionSwitcher)
        {
            _underlyingEndpoint = underlyingEndpoint;
            _namingConventionSwitcher = namingConventionSwitcher;
        }

        public IRestEndpoint Next(string path)
        {
            string convertedPath = _namingConventionSwitcher.ConvertRequestedToCoded(path);
            return _underlyingEndpoint.Next(convertedPath);
        }

        public Task<ResourceBody> GetAsync()
        {
            return _underlyingEndpoint.GetAsync();
        }

        public Task<Options> OptionsAsync()
        {
            return _underlyingEndpoint.OptionsAsync();
        }

        public Task<Feedback> UnsafeAsync(UnsafeMethod method, ResourceBody body)
        {
            return _underlyingEndpoint.UnsafeAsync(method, body);
        }

        public bool EvaluatePreconditions(IPreconditions preconditions)
        {
            return _underlyingEndpoint.EvaluatePreconditions(preconditions);
        }
    }
}