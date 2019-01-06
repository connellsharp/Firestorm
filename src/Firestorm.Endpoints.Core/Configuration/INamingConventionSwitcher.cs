namespace Firestorm.Endpoints
{
    public interface INamingConventionSwitcher
    {
        string ConvertCodedToDefault(string codedFieldName);

        string ConvertRequestedToCoded(string requestedFieldName);

        string ConvertRequestedToOutput(string requestedFieldName);
    }
}