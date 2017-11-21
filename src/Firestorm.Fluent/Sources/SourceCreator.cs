using System.Collections.Generic;

namespace Firestorm.Fluent
{
    public class SourceCreator
    {
        public ApiDirectorySource CreateSource(ApiContext apiContext, IFluentCollectionCreatorCreator creatorCreator)
        {
            var builder = new ApiBuilder();
            apiContext.OnModelCreating(builder);
            return new ApiDirectorySource(builder, creatorCreator);
        }
    }
}
