using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Engine.Defaults;
using Firestorm.Fluent.Start;
using Xunit;

namespace Firestorm.Tests.Functionality.Fluent
{
    public class FluentStartResourceFactoryTests
    {
        private readonly MemoryDataSource _memoryDataSource;

        public FluentStartResourceFactoryTests()
        {
            _memoryDataSource = new MemoryDataSource
            {
                new List<Team>
                {
                    new Team
                    {
                        Name = "Man City",
                        Division = 1,
                        Position = 1,
                        Players = new List<Player>
                        {
                            new Player { SquadNumber = 10, Name = "Sergio Aguero", Age = 29 },
                            new Player { SquadNumber = 21, Name = "David Silva", Age = 31 },
                            new Player { SquadNumber = 7, Name = "Raheem Sterling", Age = 23 }
                        }
                    },
                    new Team
                    {
                        Name = "Spurs",
                        Division = 1,
                        Position = 4,
                        Players = new List<Player>
                        {
                            new Player { SquadNumber = 20, Name = "Deli Ali", Age = 21 },
                            new Player { SquadNumber = 10, Name = "Harry Kane", Age = 24 }
                        }
                    }
                },
            };
        }

        [Fact]
        public async Task Collection_TestData_ReturnsTeamsData()
        {
            var resourceFactory = new FluentStartResourceFactory
            {
                ApiContext = new TestFluentContext(),
                DataSource = _memoryDataSource
            };

            var startResource = resourceFactory.GetStartResource(null); // TODO we don't actually use the context yet?
            var startDirectory = Assert.IsAssignableFrom<IRestDirectory>(startResource);

            var teamsCollection = startDirectory.GetCollection("teams");
            var data = await teamsCollection.QueryDataAsync(null);
            string firstTeamName = (string) data.Items.First()["name"];

            Assert.Equal("Man City", firstTeamName);
        }

        [Fact]
        public async Task Item_TestData_ReturnsTeamsData()
        {
            var resourceFactory = new FluentStartResourceFactory
            {
                ApiContext = new TestFluentContext(),
                DataSource = _memoryDataSource
            };

            var startResource = resourceFactory.GetStartResource(null); // TODO we don't actually use the context yet?
            var startDirectory = Assert.IsAssignableFrom<IRestDirectory>(startResource);

            var teamsCollection = startDirectory.GetCollection("teams");
            var item = teamsCollection.GetItem("mancity");
            var teamData = await item.GetDataAsync(null);

            string teamName = (string)teamData["name"];

            Assert.Equal("Man City", teamName);
        }

        [Fact]
        public async Task AutoRoots_TestData_ReturnsTeamsData()
        {
            var resourceFactory = new FluentStartResourceFactory
            {
                ApiContext = new AutoRootFluentContext(),
                DataSource = _memoryDataSource
            };

            var startResource = resourceFactory.GetStartResource(null); // TODO we don't actually use the context yet?
            var startDirectory = Assert.IsAssignableFrom<IRestDirectory>(startResource);

            var teamsCollection = startDirectory.GetCollection("Teams");
            var item = teamsCollection.GetItem("mancity");

            var teamData = await item.GetDataAsync(null);
            string teamName = (string)teamData["Name"];

            Assert.Equal("Man City", teamName);
        }

        [Fact]
        public async Task AutoRoots_NestingWithNullObject_ReturnsNull()
        {
            var resourceFactory = new FluentStartResourceFactory
            {
                ApiContext = new AutoRootFluentContext(),
                DataSource = _memoryDataSource
            };

            var startResource = resourceFactory.GetStartResource(null);
            var startDirectory = Assert.IsAssignableFrom<IRestDirectory>(startResource);

            var teamsCollection = startDirectory.GetCollection("Teams");
            var team = teamsCollection.GetItem("mancity");
            var players = team.GetCollection("Players");
            var player = players.GetItem(7);

            var playerData = await player.GetDataAsync(null);
            string teamName = (string)playerData["Name"];

            Assert.Equal("Raheem Sterling", teamName);
        }

        [Fact]
        public async Task AutoRoots_NestingIntoNullObject_ReturnsNull()
        {
            var resourceFactory = new FluentStartResourceFactory
            {
                ApiContext = new AutoRootFluentContext(),
                DataSource = _memoryDataSource
            };

            var startResource = resourceFactory.GetStartResource(null);
            var startDirectory = Assert.IsAssignableFrom<IRestDirectory>(startResource);

            var teamsCollection = startDirectory.GetCollection("Teams");
            var team = teamsCollection.GetItem("mancity");
            var players = team.GetCollection("Players");
            var player = players.GetItem(7);
            var team2 = player.GetItem("Team");

            var teamData = await team2.GetDataAsync(null);
            Assert.Null(teamData);

            //var name = team2.GetScalar("Name");
            //var nameData = await name.GetAsync();
            //Assert.Equal("Man City", nameData);
        }
    }
}
