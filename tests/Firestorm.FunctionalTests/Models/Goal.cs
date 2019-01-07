namespace Firestorm.FunctionalTests.Models
{
    public class Goal
    {
        public int Id { get; set; }

        public Fixture Fixture { get; set; }

        public Player Player { get; set; }
    }
}