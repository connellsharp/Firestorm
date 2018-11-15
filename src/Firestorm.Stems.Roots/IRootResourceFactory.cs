using System;
using System.Collections.Generic;
using Firestorm.Host;

namespace Firestorm.Stems.Roots
{
    /// <summary>
    /// Defines which Stem <see cref="Type"/>s are used and how to get the start resource for the <see cref="StemsStartResourceFactory"/>.
    /// </summary>
    public interface IRootResourceFactory
    {
        IEnumerable<Type> GetStemTypes(IStemConfiguration configuration);

        IRestResource GetStartResource(IStemConfiguration configuration, IRequestContext requestContext);
    }
}