using System;
using System.Linq;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Essentials.Resolvers;

namespace Firestorm.Stems.Essentials
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Based on the attribute in Web API.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class AuthorizeAttribute : FieldAttribute
    {
        private string _roles;
        private string[] _rolesSplit = new string[0];
        private string _users;
        private string[] _usersSplit = new string[0];

        /// <summary>Gets or sets the authorized roles. </summary>
        /// <returns>The roles string. </returns>
        public string Roles
        {
            get { return _roles ?? string.Empty; }
            set
            {
                _roles = value;
                _rolesSplit = SplitString(value);
            }
        }

        /// <summary>Gets or sets the authorized users. </summary>
        /// <returns>The users string. </returns>
        public string Users
        {
            get { return _users ?? string.Empty; }
            set
            {
                _users = value;
                _usersSplit = SplitString(value);
            }
        }

        public bool IsAuthorized(IRestUser user)
        {
            if (!user.IsAuthenticated)
            {
                return false;
            }

            if (_usersSplit.Length > 0 && !_usersSplit.Contains(user.Username, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            if (_rolesSplit.Length > 0 && !_rolesSplit.Any(user.IsInRole))
            {
                return false;
            }

            return true;
        }

        internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
                return new string[0];

            var split = from piece in original.Split(',')
                let trimmed = piece.Trim()
                where !string.IsNullOrEmpty(trimmed)
                select trimmed;

            return split.ToArray();
        }

        public override IAttributeResolver GetResolver()
        {
            return new AuthorizeAttributeResolver(IsAuthorized);
        }
    }
}