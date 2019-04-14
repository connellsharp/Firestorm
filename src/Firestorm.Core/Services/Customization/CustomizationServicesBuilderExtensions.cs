using System;
using JetBrains.Annotations;

namespace Firestorm.Features
{
    [PublicAPI]
    public static class CustomizationServicesBuilderExtensions
    {
        /// <summary>
        /// Registers a <see cref="T"/> type, enabling <see cref="ICustomization{T}"/> to extend this type.
        /// </summary>
        public static IServicesBuilder AddCustomizable<T>(this IServicesBuilder builder)
            where T : class, new()
        {
            return builder.AddCustomizable<T>(sp => new T());
        }

        /// <summary>
        /// Registers a <see cref="T"/> type, enabling <see cref="ICustomization{T}"/> to extend this type.
        /// </summary>
        public static IServicesBuilder AddCustomizable<T>(this IServicesBuilder builder, T instance)
            where T : class
        {
            return builder.AddCustomizable<T>(sp => instance);
        }

        /// <summary>
        /// Registers a <see cref="TAbstraction"/> type, enabling <see cref="ICustomization{TAbstraction}"/> to extend this type.
        /// </summary>
        public static IServicesBuilder AddCustomizable<TAbstraction, TImplementation>(this IServicesBuilder builder)
            where TAbstraction : class
            where TImplementation : TAbstraction, new()
        {
            return builder.AddCustomizable<TAbstraction>(sp => new TImplementation());
        }

        /// <summary>
        /// Registers a <see cref="T"/> type, enabling <see cref="ICustomization{T}"/> to extend this type.
        /// </summary>
        public static IServicesBuilder AddCustomizable<T>(this IServicesBuilder builder, Func<IServiceProvider, T> initialFactory)
            where T : class
        {
            return builder.Add<T>(sp =>
            {
                T target = initialFactory.Invoke(sp);

                foreach (ICustomization<T> customization in sp.GetServices<ICustomization<T>>())
                {
                    target = customization.Apply(target);
                }

                return target;
            });
        }

        /// <summary>
        /// Registers a <see cref="ICustomization{T}"/> to extend the <see cref="T"/> type.
        /// </summary>
        public static IServicesBuilder AddCustomization<T, TCustomization>(this IServicesBuilder builder)
            where TCustomization : class, ICustomization<T>
        {
            return builder.Add<ICustomization<T>, TCustomization>();
        }

        /// <summary>
        /// Registers a <see cref="ICustomization{T}"/> to extend the <see cref="T"/> type.
        /// </summary>
        public static IServicesBuilder AddCustomization<T, TCustomization>(this IServicesBuilder builder, TCustomization customization)
            where TCustomization : class, ICustomization<T>
        {
            return builder.Add<ICustomization<T>>(customization);
        }

        /// <summary>
        /// Registers a <see cref="ICustomization{T}"/> to extend the <see cref="T"/> type.
        /// </summary>
        public static IServicesBuilder AddCustomization<T, TExtension>(this IServicesBuilder builder, Func<IServiceProvider, TExtension> customizationFactory)
            where TExtension : class, ICustomization<T>
        {
            return builder.Add<ICustomization<T>>(customizationFactory);
        }
    }
}