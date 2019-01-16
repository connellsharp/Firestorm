using System;
using System.Threading.Tasks;
using Firestorm.Endpoints.Requests;
using Firestorm.Rest.Web;
using Firestorm.Rest.Web.Options;
using Firestorm.Host.Infrastructure;

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

        public async Task<Options> OptionsAsync()
        {
            return new Options
            {
                Description = "An item",
                AllowedMethods = new[]
                {
                    new OptionsMethod("GET", "Gets the value"),
                    new OptionsMethod("PUT", "Replaces the value"),
                    new OptionsMethod("DELETE", "Resets the value back to default")
                }
            };
        }

        public Task<Feedback> CommandAsync(UnsafeMethod method, ResourceBody body)
        {
            ICommandStrategy<IRestScalar> strategy = Context.Configuration.CommandStrategies.ForScalars.GetOrThrow(method);
            return strategy.ExecuteAsync(Scalar, Context, body);
        }

        public bool EvaluatePreconditions(IPreconditions preconditions)
        {
            return true;
            // TODO: implement precondition checks
        }
    }
}