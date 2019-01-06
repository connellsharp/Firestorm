namespace Firestorm.Fluent
{
    /// <summary>
    /// An abstract API context that can use <see cref="ApiRoot{TItem}"/>
    /// properties and allows overriding <see cref="OnApiCreating"/> to use the builder pattern.
    /// </summary>
    public abstract class ApiContext : IApiContext
    {
        protected ApiContext(ApiContextOptions options)
        {
            Options = options;
        }

        protected ApiContext()
            : this(new ApiContextOptions())
        { }

        protected internal ApiContextOptions Options { get; }

        public void CreateApi(IApiBuilder apiBuilder)
        {
            var autoConfigurer = new AutoConfigurer(Options.RootConfiguration);
            autoConfigurer.AddApiRootProperties(apiBuilder, GetType());
            
            OnApiCreating(apiBuilder);
        }

        protected virtual void OnApiCreating(IApiBuilder apiBuilder)
        {
        }
    }
}
