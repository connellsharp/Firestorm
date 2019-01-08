namespace Firestorm.Testing.Http
{
    public class TestUser : IRestUser
    {
        public const string TestUsername = "TestUsername";
        public const string TestRole = "TestRole";
        
        public string Username
        {
            get { return TestUsername; }
        }

        public bool IsAuthenticated
        {
            get { return Username != null; }
        }

        public bool IsInRole(string role)
        {
            return role == TestRole;
        }
    }
}