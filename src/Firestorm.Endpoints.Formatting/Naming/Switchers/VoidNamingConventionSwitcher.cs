namespace Firestorm.Endpoints.Formatting.Naming
{
    /// <summary>
    /// A naming <see cref="INamingConventionSwitcher"/> implementation that does nothing.
    /// It always returns the same string it was given.
    /// </summary>
    public class VoidNamingConventionSwitcher : INamingConventionSwitcher
    {
        public string ConvertCodedToDefault(string codedFieldName)
        {
            return codedFieldName;
        }

        public string ConvertRequestedToCoded(string requestedFieldName)
        {
            return requestedFieldName;
        }

        public string ConvertRequestedToOutput(string requestedFieldName)
        {
            return requestedFieldName;
        }
    }
}