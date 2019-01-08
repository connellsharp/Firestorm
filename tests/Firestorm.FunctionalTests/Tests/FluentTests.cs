using Firestorm.FunctionalTests.Setup;
using Xunit;

namespace Firestorm.FunctionalTests.Tests
{
    public class FluentTests
    {
        [Collection(nameof(FluentCollectionDefinition))]
        public class RootDirectoryTests : RootDirectoryTestsBase
        {
            public RootDirectoryTests(FluentTestFixture fixture)
                : base(fixture)
            {
            }
        }
        
        [Collection(nameof(FluentCollectionDefinition))]
        public class NavigationPropertyTests : NavigationPropertyTestsBase
        {
            public NavigationPropertyTests(FluentTestFixture fixture)
                : base(fixture)
            {
            }
        }

        [Collection(nameof(FluentCollectionDefinition))]
        public class PaginationTests : PaginationTestsBase
        {
            public PaginationTests(FluentTestFixture fixture)
                : base(fixture)
            {
            }
        }
    }
}