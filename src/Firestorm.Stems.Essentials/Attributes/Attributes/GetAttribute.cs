using System;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Definitions;
using Firestorm.Stems.Essentials.Resolvers;

namespace Firestorm.Stems.Essentials
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class GetAttribute : FieldAttribute
    {
        public GetAttribute()
        { }

        public GetAttribute(string name)
            : base(name)
        { }

        public GetAttribute(Display display)
        {
            Display = display;
        }

        public GetAttribute(string name, Display display)
            : base(name)
        {
            Display = display;
        }

        /// <summary>
        /// Defines the nesting levels that will display this field if the field names are not specified in the request.
        /// </summary>
        public Display? Display { get; set; }

        /// <summary>
        /// The name of the member that defines an Expression.
        /// For each item, the result of that expression is given as the argument to this getter method.
        /// </summary>
        public string Argument { get; set; }

        public override string GetDefaultName(string memberName)
        {
            return memberName.TrimStart("Get");
        }

        public override IAttributeResolver GetResolver()
        {
            return new GetterAttributeResolver(Argument, Display);
        }
    }
}