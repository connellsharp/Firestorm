using System.IO;
using JetBrains.Annotations;

namespace Firestorm.Endpoints.Formatting
{
    public interface IContentReader
    {
        [CanBeNull] Stream GetContentStream();
        [CanBeNull] string GetMimeType();
    }
}