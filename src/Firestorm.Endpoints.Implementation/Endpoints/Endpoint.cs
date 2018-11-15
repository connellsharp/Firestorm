using System;
using JetBrains.Annotations;

namespace Firestorm.Endpoints
{
    /// <summary>
    /// Provides a utility to get an <see cref="IRestEndpoint"/> implementation.
    /// </summary>
    public static class Endpoint
    {
        public static IRestEndpoint GetFromResource(IEndpointContext endpointContext, [NotNull] IRestResource resource)
        {
            if (resource == null)
                throw new ArgumentNullException(nameof(resource));

            switch (resource)
            {
                case IRestCollection collection:
                    return new RestCollectionEndpoint(endpointContext, collection);

                case IRestItem item:
                    return new RestItemEndpoint(endpointContext, item);

                case IRestScalar scalar:
                    return new RestScalarEndpoint(endpointContext, scalar);

                case IRestDictionary dictionary:
                    return new RestDictionaryEndpoint(endpointContext, dictionary);

                case IRestDirectory directory:
                    return new RestDirectoryEndpoint(endpointContext, directory);

                default:
                        throw new IncorrectResourceTypeException(resource.GetType());
            }
        }
    }
}