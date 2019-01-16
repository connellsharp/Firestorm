using System.Threading.Tasks;
using Firestorm.Rest.Web;
using Firestorm.Endpoints.Strategies;
using Firestorm.Endpoints.Tests.Stubs;
using Moq;
using Xunit;

namespace Firestorm.Endpoints.Tests.Strategies
{
    public class AddToCollectionStrategyTests
    {
        private readonly Mock<IRestCollection> _collectionMock;

        public AddToCollectionStrategyTests()
        {
            _collectionMock = new Mock<IRestCollection>();

            _collectionMock
                .Setup(c => c.AddAsync(It.IsAny<RestItemData>()))
                .ReturnsAsync(new CreatedItemAcknowledgment(null));
        }

        [Fact]
        public async Task ExecuteAdd_IncreasesCount()
        {
            var newItemData = new RestItemData();
            var strategy = new AddToCollectionStrategy();
            await strategy.ExecuteAsync(_collectionMock.Object, new TestEndpointContext(), new ItemBody(newItemData));

            _collectionMock.Verify(c => c.AddAsync(It.IsAny<RestItemData>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAddMany_IncreasesCount()
        {
            var newItemData = new RestItemData();
            var strategy = new AddToCollectionStrategy();
            var items = new[] { newItemData, newItemData, newItemData };
            await strategy.ExecuteAsync(_collectionMock.Object, new TestEndpointContext(), new CollectionBody(items, null));
            
            _collectionMock.Verify(c => c.AddAsync(It.IsAny<RestItemData>()), Times.Exactly(3));
        }
    }
}
