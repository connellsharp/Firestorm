using System.Collections.Generic;
using System.Linq;
using Firestorm.Tests.Examples.Football.Models;

namespace Firestorm.Tests.Examples.Football.Data
{
    public static class DbInitializer
    {
        public static void Initialize(FootballDbContext dbContext)
        {
            if (!dbContext.Leagues.Any())
            {
                dbContext.Leagues.Add(new League
                {
                    Name = "Premier League",
                    Teams = new List<Team>
                    {
                        new Team
                        {
                            Name = "Man City",
                            FoundedYear = 2000
                        },
                        new Team
                        {
                            Name = "Chelsea",
                            FoundedYear = 2000
                        },
                        new Team
                        {
                            Name = "Spurs",
                            FoundedYear = 2000
                        },
                        new Team
                        {
                            Name = "Liverpool",
                            FoundedYear = 2000
                        },
                    }
                });

                dbContext.SaveChanges();
            }

            if (!dbContext.Fixtures.Any())
            {
                dbContext.Teams.Find(1).Fixtures = new List<FixtureTeam>
                {
                    new FixtureTeam { IsHome = true, Fixture = new Fixture { Teams = new List<FixtureTeam> { new FixtureTeam { TeamId = 2 } } } }
                    //new FixtureTeam { IsHome = false, Fixture = new Fixture { Teams = new List<FixtureTeam> { new FixtureTeam { TeamId = 3 } } } },
                };

                dbContext.SaveChanges();
            }
        }
    }
}