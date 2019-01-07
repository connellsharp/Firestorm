namespace Firestorm.Fluent.IntegrationTests.Models
{
    public class Player
    {
        public string Name { get; set; }

        public string Alias { get; set; }

        public int Age { get; set; }

        public int SquadNumber { get; set; }

        public Team Team { get; set; }
    }
}