namespace Firestorm.Fluent
{
    public interface IApiContext
    {
        /// <summary>
        /// Creates a new API by instructing the given <see cref="IApiBuilder"/>.
        /// </summary>
        void CreateApi(IApiBuilder apiBuilder);
    }
}