using System;
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

        public async Task<ResourceBody> GetAsync()
        {
            ResourceBody body = await _underlyingEndpoint.GetAsync();
            new BodyKeyConverter(_namingConventionSwitcher.ConvertCodedToDefault).ChangeKeys(body);
            return body;
        }

        public async Task<Options> OptionsAsync()
        {
            Options options = await _underlyingEndpoint.OptionsAsync();
            new BodyKeyConverter(_namingConventionSwitcher.ConvertCodedToDefault).ChangeKeys(options);
            return options;
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

    internal class BodyKeyConverter
    {
        private readonly Func<string, string> _transformFunc;

        public BodyKeyConverter(Func<string, string> transformFunc)
        {
            _transformFunc = transformFunc;
        }

        internal void ChangeKeys(ResourceBody body)
        {
            switch (body)
            {
                case CollectionBody collectionBody:
                    foreach (var item in collectionBody.Items)
                        ChangeItemKeys(item);
            }
        }

        private void ChangeItemKeys(RestItemData item)
        {
        }
    }
}