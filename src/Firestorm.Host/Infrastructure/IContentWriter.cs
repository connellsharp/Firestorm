using System.Threading.Tasks;

namespace Firestorm.Host.Infrastructure
{
    public interface IContentWriter
    {
        void SetMimeType(string mimeType);

        void SetLength(int bytesLength);

        Task WriteBytesAsync(byte[] bytes);
    }
}