using System.Configuration;

namespace Headtrip.Secrets
{
    public static class InternalApiKeys
    {
        public static string? GameServerApiKey { 
            get
            {
                return ConfigurationManager.AppSettings["GameServerApiKey"];
            }
        }

        public static string? WebsiteApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["WebsiteApiKey"];
            }
        }
    }
}