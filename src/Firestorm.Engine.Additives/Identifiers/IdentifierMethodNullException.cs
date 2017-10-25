namespace Firestorm.Engine.Additives.Identifiers
{
    internal class IdentifierMethodNullException : RestApiException
    {
        public IdentifierMethodNullException()
            : base(ErrorStatus.NotFound, "Identifier method returned null.")
        {
        }
    }
}