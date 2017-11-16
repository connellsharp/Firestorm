using System;
using System.Collections.Generic;
using System.Linq;

namespace Firestorm.Stems.Roots
{
    /// <summary>
    /// Manually gets the given types.
    /// </summary>
    public class ManualTypeGetter : ITypeGetter
    {
        private readonly IEnumerable<Type> _types;

        /// <summary>
        /// Manually gets the given <see cref="types"/>
        /// </summary>
        public ManualTypeGetter(params Type[] types)
            : this(types.AsEnumerable())
        { }

        /// <summary>
        /// Manually gets the given <see cref="types"/>
        /// </summary>
        public ManualTypeGetter(IEnumerable<Type> types)
        {
            _types = types;
        }

        public IEnumerable<Type> GetAvailableTypes()
        {
            return _types;
        }
    }
}