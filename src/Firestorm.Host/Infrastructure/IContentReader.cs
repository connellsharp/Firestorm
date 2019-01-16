using System.IO;
using JetBrains.Annotations;

namespace Firestorm.Host.Infrastructure
{
    public interface IContentReader
    {
        [CanBeNull] 
        Stream GetContentStream();
        
        [CanBeNull] 
        string GetMimeType();
    }
}