using Firestorm.Fluent;

namespace Firestorm.Tests.Functionality.Fluent
{
    public class AutoRootFluentContext : ApiContext
    {
        public AutoRootFluentContext()
        {
        }

        public ApiRoot<Player> Players { get; set; }

        public ApiRoot<Team> Teams { get; set; }

        protected override void OnApiCreating(IApiBuilder apiBuilder)
        {
            apiBuilder.Item<Team>(i =>
            {
                i.Identifier(t => t.Name.Replace(" ", "").ToLower())
                    .HasName("key");
            });
        }
    }
}