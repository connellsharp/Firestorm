using System.IO;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints.Tests.Start.Readers
{
    internal class EmptyReader : IContentReader
    {
        public Stream GetContentStream()
        {
            return null;
        }

        public string GetMimeType()
        {
            return null;
        }
    }
}