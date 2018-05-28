using Firestorm.Data;
using Firestorm.Stems.Roots.Combined;

namespace Firestorm.Stems.Roots.DataSource
{
    public class DataSourceRootResourceFactory : RootResourceFactoryBase
    {
        public IDataSource DataSource { get; set; }

        public ITypeGetter StemTypeGetter { get; set; }

        public DataSourceRootAttributeBehavior RootBehavior { get; set; }

        protected override IRootStartInfoFactory CreateStartInfoFactory()
        {
            return new DataSourceVaseStartInfoFactory(DataSource, StemTypeGetter, RootBehavior);
        }
    }
    
    public enum DataSourceRootAttributeBehavior
    {
        /// <summary>
        /// Only use Stems marked with the <see cref="DataSourceRootAttribute"/>.
        /// </summary>
        OnlyUseAllowedStems,

        /// <summary>
        /// Use all Stems except those marked with the <see cref="NoDataSourceRootAttribute"/>.
        /// </summary>
        UseAllStemsExceptDisallowed
    }
}