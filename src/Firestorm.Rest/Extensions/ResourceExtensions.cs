using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Firestorm
{
    public static class ResourceExtensions
    {
        public static IRestCollection GetCollection([NotNull] this IRestDirectory directory, [NotNull] string startResourceName)
        {
            return directory.GetChild(startResourceName).AsCollection();
        }

        public static IRestItem GetItem([NotNull] this IRestDirectory directory, [NotNull] string startResourceName)
        {
            return directory.GetChild(startResourceName).AsItem();
        }

        public static IRestScalar GetScalar([NotNull] this IRestDirectory directory, [NotNull] string startResourceName)
        {
            return directory.GetChild(startResourceName).AsScalar();
        }

        public static IRestCollection GetCollection([NotNull] this IRestItem item, [NotNull] string fieldName)
        {
            return item.GetField(fieldName).AsCollection();
        }

        public static IRestItem GetItem([NotNull] this IRestItem item, [NotNull] string fieldName)
        {
            return item.GetField(fieldName).AsItem();
        }

        public static IRestScalar GetScalar([NotNull] this IRestItem item, [NotNull] string fieldName)
        {
            return item.GetField(fieldName).AsScalar();
        }

        public static IRestItem GetItem([NotNull] this IRestCollection collection, int identifier)
        {
            return collection.GetItem(identifier.ToString());
        }

        public static Task<CreatedItemAcknowledgment> AddAsync([NotNull] this IRestCollection collection, object anonymousObj)
        {
            return collection.AddAsync(new RestItemData(anonymousObj));
        }

        public static Task<Acknowledgment> EditAsync([NotNull] this IRestItem item, object anonymousObj)
        {
            return item.EditAsync(new RestItemData(anonymousObj));
        }

        private static IRestCollection AsCollection([NotNull] this IRestResource resource)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            
            if (resource is IRestCollection collection) return collection;

            throw new ArgumentException("Resource was not a collection.", nameof(resource));
        }

        private static IRestItem AsItem([NotNull] this IRestResource resource)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));

            if (resource is IRestItem item) return item;

            throw new ArgumentException("Resource was not an item.", nameof(resource));
        }

        private static IRestScalar AsScalar([NotNull] this IRestResource resource)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));

            if (resource is IRestScalar scalar) return scalar;

            throw new ArgumentException("Resource was not a scalar.", nameof(resource));
        }
    }
}
