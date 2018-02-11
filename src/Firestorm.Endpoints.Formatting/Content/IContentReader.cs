using System.IO;

namespace Firestorm.Endpoints.Formatting
{
    public interface IContentReader
    {
        Stream GetContentStream();
        string GetMimeType();
    }
}