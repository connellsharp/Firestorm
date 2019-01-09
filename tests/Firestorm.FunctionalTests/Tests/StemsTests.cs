using Firestorm.FunctionalTests.Setup;
using Xunit;

namespace Firestorm.FunctionalTests.Tests
{
    public class StemsTests
    {
        [Collection(nameof(StemsCollectionDefinition))]
        public class RootDirectoryTests : RootDirectoryTestsBase
        {
            public RootDirectoryTests(StemsTestFixture fixture)
                : base(fixture)
            {
            }
        }
        
        [Collection(nameof(StemsCollectionDefinition))]
        public class NavigationPropertyTests : NavigationPropertyTestsBase
        {
            public NavigationPropertyTests(StemsTestFixture fixture)
                : base(fixture)
            {
            }
        }

        [Collection(nameof(StemsCollectionDefinition))]
        public class PaginationTests : PaginationTestsBase
        {
            public PaginationTests(StemsTestFixture fixture)
                : base(fixture)
            {
            }
        }
    }
}