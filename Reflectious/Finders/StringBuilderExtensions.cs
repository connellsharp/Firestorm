using System;
using System.Text;

namespace Firestorm
{
    internal static class StringBuilderExtensions
    {
        internal static void AppendFullTypeNames(this StringBuilder builder, Type[] genericArguments)
        {
            if (genericArguments != null)
                foreach (var type in genericArguments)
                    builder.Append(type.GetHashCode());
        }
    }
}