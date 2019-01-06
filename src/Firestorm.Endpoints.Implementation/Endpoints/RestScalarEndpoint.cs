using System;
using System.Threading.Tasks;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;
using Firestorm.Endpoints.Preconditions;
using Firestorm.Endpoints.Strategies;

namespace Firestorm.Endpoints
{
    internal class RestScalarEndpoint : IRestEndpoint
    {
        internal RestScalarEndpoint(IEndpointContext endpointContext, IRestScalar scalar)
        {
            Context = endpointContext;
            Scalar = scalar;
        }

        public IEndpointContext Context { get; }

        public IRestScalar Scalar { get; }

        public IRestEndpoint Next(INextPath nextPath)
        {
            throw new InvalidOperationException("No further paths are allowed after a scalar property.");
        }

        public async Task<ResourceBody> GetAsync(IRestCollectionQuery query)
        {
            object value = await Scalar.GetAsync();
            return new ScalarBody(value);
        }

        public Task<Options> OptionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Feedback> UnsafeAsync(UnsafeMethod method, ResourceBody body)
        {
            IUnsafeRequestStrategy<IRestScalar> strategy = Context.Configuration.RequestStrategies.ForScalars.GetOrThrow(method);
            return strategy.ExecuteAsync(Scalar, Context, body);
        }

        public bool EvaluatePreconditions(IPreconditions preconditions)
        {
            return true;
            // TODO: implement precondition checks
        }
    }
}