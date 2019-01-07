using System;

namespace Firestorm.Testing.Data
{
    public static class DbConnectionStrings
    {
        public static string Resolve(string databaseName)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                return
                    $@"Server=(localdb)\mssqllocaldb;Database={databaseName};Trusted_Connection=True;ConnectRetryCount=0";

            return
                $@"Server=localhost,1434;Database={databaseName};MultipleActiveResultSets=true;User Id=sa;Password=Password_123456;";
        }
    }
}