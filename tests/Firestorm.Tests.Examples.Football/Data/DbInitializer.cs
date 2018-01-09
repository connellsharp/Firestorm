using System.Collections.Generic;
using Firestorm.Tests.Examples.Football.Models;

namespace Firestorm.Tests.Examples.Football.Data
{
    public static class DbInitializer
    {
        public static void Initialize(FootballDbContext dbContext)
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
                }
            });

            dbContext.SaveChanges();
        }
    }
}