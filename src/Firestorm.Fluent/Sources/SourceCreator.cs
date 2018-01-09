using System.Linq;
using System.Reflection;

namespace Firestorm.Fluent.Sources
{
    public class SourceCreator
    {
        public IApiDirectorySource CreateSource(RestContext restContext, IApiBuilder builder)
        {
            AddRoots(restContext, builder);

            restContext.OnApiCreating(builder);

            return builder.BuildSource();
        }

        private void AddRoots(RestContext restContext, IApiBuilder builder)
        {
            //foreach (PropertyInfo property in restContext.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            //{
            //    property.
            //    builder.Item<>()
            //}
        }
    }
}
