using System.Threading.Tasks;

namespace Firestorm.Endpoints.Tests.Stubs
{
    internal class TestRestDirectory : IRestDirectory
    {
        public IRestResource GetChild(string startResourceName)
        {
            switch (startResourceName)
            {
                case "artists":
                    return new ArtistMemoryCollection();

                default:
                    return null;
            }
        }

        public Task<RestDirectoryInfo> GetInfoAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}