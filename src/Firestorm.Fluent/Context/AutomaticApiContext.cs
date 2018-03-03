using System;
using System.Collections.Generic;

namespace Firestorm.Fluent
{
    /// <summary>
    /// An <see cref="IApiContext"/> that automatically configures the given item types using the <see cref="AutoConfiguration"/>.
    /// </summary>
    public class AutomaticApiContext : IApiContext
    {
        private readonly IEnumerable<Type> _itemTypes;
        private readonly AutoConfiguration _configuration;

        public AutomaticApiContext(IEnumerable<Type> itemTypes, AutoConfiguration configuration)
        {
            _itemTypes = itemTypes;
            _configuration = configuration;
        }

        public void CreateApi(IApiBuilder apiBuilder)
        {
            var autoConfigurer = new AutoConfigurer(_configuration);
            
            foreach (Type itemType in _itemTypes)
            {
                autoConfigurer.AddRootItem(apiBuilder, itemType, null);
            }
        }
    }
}