using System.Configuration;

namespace Headtrip.Secrets
{
    public static class InternalApiKeys
    {
        public static string UnrealGameServerApiKey { 
            get
            {
                var result = ConfigurationManager.AppSettings["UnrealGameServerApiKey"];
                if (result == null)
                    throw new Exception("UnrealGameServerApiKey MISSING FROM CONFIGURATION FILE");

                return result;
            }
        }

        public static string? UnrealUnrealServiceApiKey
        {
            get
            {
                var result = ConfigurationManager.AppSettings["UnrealUnrealServiceApiKey"];
                if (result == null)
                    throw new Exception("UnrealUnrealServiceApiKey MISSING FROM CONFIGURATION FILE");

                return result;
            }
        }

        public static string ApiKeyHeaderName { get; private set; } = "ht-services-api-key";


        public static string? GetApiKey(string keyName) =>
            ConfigurationManager.AppSettings[keyName];
    }
}