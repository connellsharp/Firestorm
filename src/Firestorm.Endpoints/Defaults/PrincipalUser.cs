using System.Security.Principal;
using Firestorm.Core;

namespace Firestorm.Endpoints
{
    public class PrincipalUser : IRestUser
    {
        private readonly IPrincipal _principal;

        public PrincipalUser(IPrincipal principal)
        {
            _principal = principal;
        }

        public string Username
        {
            get { return _principal?.Identity?.Name; }
        }

        public bool IsAuthenticated
        {
            get { return _principal?.Identity?.IsAuthenticated ?? false; }
        }

        public bool IsInRole(string role)
        {
            return _principal.IsInRole(role);
        }
    }
}