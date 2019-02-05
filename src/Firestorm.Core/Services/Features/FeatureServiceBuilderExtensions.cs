using System.Collections.Generic;

namespace Firestorm.Features
{
    public static class FeatureServiceBuilderExtensions
    {
        public static IFirestormServicesBuilder EnableFeatures<T>(this IFirestormServicesBuilder builder)
            where T : class, new()
        {
            return builder
                .Add<T>(sp =>
                {
                    var target = new T();

                    foreach (IFeature<T> feature in sp.GetService<IEnumerable<IFeature<T>>>())
                    {
                        feature.AddTo(target);
                    }

                    return target;
                });
        }

        public static IFirestormServicesBuilder AddFeature<T, TFeature>(this IFirestormServicesBuilder builder)
            where TFeature : class, IFeature<T>
        {
            return builder.Add<IFeature<T>, TFeature>();
        }
    }
}