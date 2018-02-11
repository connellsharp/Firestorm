using System.Collections.Generic;

namespace Firestorm.Endpoints.Naming
{
    /// <summary>
    /// The object used to convert from .NET Stem member naming conventions to client-side API conventions.
    /// </summary>
    public class NamingConventionSwitcher : INamingConventionSwitcher
    {
        public ICaseConvention CodedCase { get; set; }

        public ICaseConvention DefaultOutputCase { get; set; }

        public IEnumerable<ICaseConvention> AllowedCases { get; set; }

        public NamingConventionSwitcher(ICaseConvention codedCase, ICaseConvention defaultOutputCase)
        {
            CodedCase = codedCase;
            DefaultOutputCase = defaultOutputCase;
            AllowedCases = new[] { defaultOutputCase };
        }

        public NamingConventionSwitcher(ICaseConvention codedCase, ICaseConvention defaultOutputCase, IEnumerable<ICaseConvention> allowedCases)
        {
            CodedCase = codedCase;
            DefaultOutputCase = defaultOutputCase;
            AllowedCases = allowedCases;
        }

        public string ConvertCodedToDefault(string codedFieldName)
        {
            return ConvertSpecifiedToDefault(codedFieldName, CodedCase);
        }

        public string ConvertSpecifiedToDefault(string fieldName, ICaseConvention caseConvention)
        {
            IEnumerable<string> words = caseConvention.Split(fieldName);
            return DefaultOutputCase.Make(words);
        }

        public string ConvertRequestedToCoded(string requestedFieldName)
        {
            return ConvertRequested(requestedFieldName, CodedCase);
        }

        public string ConvertRequestedToOutput(string requestedFieldName)
        {
            return ConvertRequested(requestedFieldName, DefaultOutputCase);
        }

        private string ConvertRequested(string requestedFieldName, ICaseConvention caseConvention)
        {
            foreach (ICaseConvention allowedCase in AllowedCases)
            {
                if (allowedCase.IsCase(requestedFieldName))
                {
                    IEnumerable<string> words = allowedCase.Split(requestedFieldName);
                    return caseConvention.Make(words);
                }
            }

            return null;
        }
    }
}