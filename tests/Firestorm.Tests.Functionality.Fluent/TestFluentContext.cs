using Firestorm.Fluent;

namespace Firestorm.Tests.Functionality.Fluent
{
    public class TestFluentContext : ApiContext
    {
        protected override void OnModelCreating(IApiBuilder apiBuilder)
        {
            apiBuilder.Item<Player>(i =>
            {
                i.Field(t => t.Name)
                    .HasName("name");

                i.Field(t => t.Alias)
                    .HasName("alias");

                i.Field(t => t.SquadNumber)
                    .HasName("number");

                i.Field(t => t.Age)
                    .HasName("age");
            });

            apiBuilder.Item<Team>(i =>
            {
                i.RootName = "teams";

                i.Field(t => t.Name)
                    .HasName("name");

                i.Field(t => t.Division)
                    .HasName("division");

                i.Field(t => t.Position)
                    .HasName("position");

                i.Field(t => t.Players)
                    .HasName("players");
            });
        }
    }
}