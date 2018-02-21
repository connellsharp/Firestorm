using System.Linq;

namespace Firestorm.Stems.Roots
{
    public static class NamedResourcesUtility
    {
        /// <summary>
        /// Creates a <see cref="RestDirectoryInfo"/> object containing an entry for each item in the dictionary.
        /// All directories will return Collection types at the moment, until other types are supported in future.
        /// </summary>
        /// <param name="namedTypeDictionary"></param>
        /// <returns></returns>
        public static RestDirectoryInfo CreateDirectoryInfo(this NamedTypeDictionary namedTypeDictionary)
        {
            var resourceInfos = from name in namedTypeDictionary.GetAllNames()
                                //let convertedName = name // TODO caps?
                                select new RestResourceInfo(name, ResourceType.Collection);

            return new RestDirectoryInfo(resourceInfos);
        }
    }
}