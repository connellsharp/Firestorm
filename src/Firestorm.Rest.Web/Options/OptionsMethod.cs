namespace Firestorm.Rest.Web.Options
{
    public class OptionsMethod
    {
        public OptionsMethod()
        {
        }
        
        public OptionsMethod(string verb, string description)
        {
            Verb = verb;
            Description = description;
        }

        public string Verb { get; set; }

        public string Description { get; set; }
    }
}