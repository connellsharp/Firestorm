using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Authorization;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Defaults;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using Firestorm.Tests.Unit.Engine.Models;
using Ninject;
using Xunit;

namespace Firestorm.Tests.Unit.Engine
{
    public class DependencyInjectionTests
    {
        [Fact]
        public async Task DefaultContentWithMemoryRepository()
        {
            IKernel kernel = new StandardKernel();

            kernel.Bind(typeof(IEngineContext<>)).To(typeof(InjectedEngineContext<>));

            kernel.Bind(typeof(IDataTransaction)).To(typeof(VoidTransaction));
            kernel.Bind(typeof(IEngineRepository<Artist>)).To(typeof(ArtistMemoryRepository));

            kernel.Bind(typeof(IAuthorizationChecker<>)).To(typeof(AllowAllAuthorizationChecker<>));

            kernel.Bind(typeof(IIdentifierProvider<>)).To(typeof(SingleIdentifierProvider<>));

            kernel.Bind(typeof(IIdentifierInfo<Artist>)).ToConstant(new ExpressionIdentifierInfo<Artist, int>(a => a.ID));
            kernel.Bind(typeof(IFieldProvider<Artist>)).ToConstant(GetArtistFieldMappings());

            var collection = kernel.Get<EngineRestCollection<Artist>>();
            var itemData = await collection.GetItem("123").GetDataAsync(new[] { "name" });
            Assert.Equal(itemData["name"], TestRepositories.ArtistName);
        }

        private static FieldDictionary<Artist> GetArtistFieldMappings()
        {
            return new FieldDictionary<Artist>
            {
                {"id", a => a.ID},
                {"name", a => a.Name}
            };
        }
    }
}
