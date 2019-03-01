using System;
using JetBrains.Annotations;

namespace Firestorm.Features
{
    [PublicAPI]
    public static class FeatureServicesBuilderExtensions
    {
        /// <summary>
        /// Registers a <see cref="T"/> type, enabling Features to extend this type.
        /// </summary>
        public static IServicesBuilder AddWithFeatures<T>(this IServicesBuilder builder)
            where T : class, new()
        {
            return builder.AddWithFeatures<T>(sp => new T());
        }

        /// <summary>
        /// Registers a <see cref="T"/> type, enabling Features to extend this type.
        /// </summary>
        public static IServicesBuilder AddWithFeatures<T>(this IServicesBuilder builder, T instance)
            where T : class
        {
            return builder.AddWithFeatures<T>(sp => instance);
        }

        /// <summary>
        /// Registers a <see cref="TAbstraction"/> type, enabling Features to extend this type.
        /// </summary>
        public static IServicesBuilder AddWithFeatures<TAbstraction, TImplementation>(this IServicesBuilder builder)
            where TAbstraction : class
            where TImplementation : TAbstraction, new()
        {
            return builder.AddWithFeatures<TAbstraction>(sp => new TImplementation());
        }

        /// <summary>
        /// Registers a <see cref="T"/> type, enabling Features to extend this type.
        /// </summary>
        public static IServicesBuilder AddWithFeatures<T>(this IServicesBuilder builder, Func<IServiceProvider, T> initialFactory)
            where T : class
        {
            return builder.Add<T>(sp =>
            {
                T target = initialFactory.Invoke(sp);

                foreach (IFeature<T> feature in sp.GetServices<IFeature<T>>())
                {
                    target = feature.Apply(target);
                }

                return target;
            });
        }

        /// <summary>
        /// Registers a Feature to extend the <see cref="T"/> type.
        /// </summary>
        public static IServicesBuilder AddFeature<T, TFeature>(this IServicesBuilder builder)
            where TFeature : class, IFeature<T>
        {
            return builder.Add<IFeature<T>, TFeature>();
        }

        /// <summary>
        /// Registers a Feature to extend the <see cref="T"/> type.
        /// </summary>
        public static IServicesBuilder AddFeature<T, TFeature>(this IServicesBuilder builder, TFeature feature)
            where TFeature : class, IFeature<T>
        {
            return builder.Add<IFeature<T>>(feature);
        }

        /// <summary>
        /// Registers a Feature to extend the <see cref="T"/> type.
        /// </summary>
        public static IServicesBuilder AddFeature<T, TFeature>(this IServicesBuilder builder, Func<IServiceProvider, TFeature> featureFactory)
            where TFeature : class, IFeature<T>
        {
            return builder.Add<IFeature<T>>(featureFactory);
        }
    }
}