using System;
using System.Configuration;

namespace KnihovnaDemo
{
    public static class Config
    {
        public static string ApiBaseUrl
        {
            get
            {
                string url = ConfigurationManager.AppSettings["ApiBaseUrl"];
                if (string.IsNullOrEmpty(url))
                {
                    // Výchozí hodnota, pokud v konfiguraci není
                    return "http://localhost:5001";
                }
                return url;
            }
        }

        public static string Environment
        {
            get
            {
                string env = ConfigurationManager.AppSettings["Environment"];
                if (string.IsNullOrEmpty(env))
                {
                    return "Local";
                }
                return env;
            }
        }
        
        public static bool IsDockerEnvironment => Environment.Equals("Docker", StringComparison.OrdinalIgnoreCase);
    }
} 