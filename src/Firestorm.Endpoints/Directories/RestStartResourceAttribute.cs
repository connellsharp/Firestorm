using System;
using JetBrains.Annotations;

namespace Firestorm.Endpoints.Directories
{
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RestStartResourceAttribute : Attribute
    {
        public RestStartResourceAttribute(string directoryName)
        {
            DirectoryName = directoryName;
        }

        public string DirectoryName { get; }
    }
}