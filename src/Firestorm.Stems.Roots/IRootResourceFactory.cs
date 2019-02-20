using System;
using System.Collections.Generic;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Stems.Roots
{
    /// <summary>
    /// Defines which Stem <see cref="Type"/>s are used and how to get the start resource for the <see cref="StemsStartResourceFactory"/>.
    /// </summary>
    public interface IRootResourceFactory
    {
        IEnumerable<Type> GetStemTypes(IStemsCoreServices configuration);

        IRestResource GetStartResource(IStemsCoreServices configuration, IRequestContext requestContext);
    }
}