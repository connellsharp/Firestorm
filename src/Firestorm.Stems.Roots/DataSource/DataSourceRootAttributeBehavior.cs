namespace Firestorm.Stems.Roots.DataSource
{
    public enum DataSourceRootAttributeBehavior
    {
        /// <summary>
        /// Use all Stems except those marked with the <see cref="NoDataSourceRootAttribute"/>.
        /// </summary>
        UseAllStemsExceptDisallowed,

        /// <summary>
        /// Only use Stems marked with the <see cref="DataSourceRootAttribute"/>.
        /// </summary>
        OnlyUseAllowedStems
    }
}