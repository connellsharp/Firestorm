using JetBrains.Annotations;

namespace Firestorm.Features
{
    [PublicAPI]
    public static class FeatureServiceBuilderExtensions
    {
        /// <summary>
        /// Registers a <see cref="T"/> type, enabling Features to extend this type.
        /// </summary>
        public static IFirestormServicesBuilder AddWithFeatures<T>(this IFirestormServicesBuilder builder)
            where T : class, new()
        {
            return builder.AddWithFeatures(new T());
        }
        
        /// <summary>
        /// Registers a <see cref="T"/> type, enabling Features to extend this type.
        /// </summary>
        public static IFirestormServicesBuilder AddWithFeatures<T>(this IFirestormServicesBuilder builder, T instance)
            where T : class
        {
            return builder.AddWithFeatures<T, T>(instance);
        }
        
        /// <summary>
        /// Registers a <see cref="TAbstraction"/> type, enabling Features to extend this type.
        /// </summary>
        public static IFirestormServicesBuilder AddWithFeatures<TAbstraction, TImplementation>(this IFirestormServicesBuilder builder, TImplementation instance)
            where TAbstraction : class
            where TImplementation : TAbstraction
        {
            return builder
                .Add<TAbstraction>(sp =>
                {
                    TAbstraction target = instance;

                    foreach (IFeature<TAbstraction> feature in sp.GetServices<IFeature<TAbstraction>>())
                    {
                        target = feature.AddTo(target);
                    }

                    return target;
                });
        }

        /// <summary>
        /// Registers a Feature to extend the <see cref="T"/> type.
        /// </summary>
        public static IFirestormServicesBuilder AddFeature<T, TFeature>(this IFirestormServicesBuilder builder)
            where TFeature : class, IFeature<T>
        {
            return builder.Add<IFeature<T>, TFeature>();
        }

        /// <summary>
        /// Registers a Feature to extend the <see cref="T"/> type.
        /// </summary>
        public static IFirestormServicesBuilder AddFeature<T, TFeature>(this IFirestormServicesBuilder builder, TFeature feature)
            where TFeature : class, IFeature<T>
        {
            return builder.Add<IFeature<T>>(feature);
        }
    }
}