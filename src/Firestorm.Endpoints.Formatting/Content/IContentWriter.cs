using System.Threading.Tasks;

namespace Firestorm.Endpoints.Formatting
{
    public interface IContentWriter
    {
        void SetMimeType(string mimeType);

        void SetLength(int bytesLength);

        Task WriteBytesAsync(byte[] bytes);
    }
}