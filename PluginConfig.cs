using Rocket.API;

namespace KitsLimiter
{
    public class MyConfig : IRocketPluginConfiguration
    {
        public string DatabaseAddress;
        public string DatabaseUsername;
        public string DatabasePassword;
        public string DatabaseName;
        public string DatabaseTableName;
        public ushort DatabasePort;
        public void LoadDefaults()
        {
            DatabasePort = 3306;
            DatabaseAddress = "localhost";
            DatabaseName = "unturned";
            DatabaseTableName = "kitslimiter.mysqlkits";
            DatabaseUsername = "root";
            DatabasePassword = "password";
        }
    }
}