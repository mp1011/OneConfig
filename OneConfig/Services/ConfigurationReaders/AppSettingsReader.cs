using OneConfig.Models;
using OneConfig.Services.Interfaces;
using System.Configuration;

namespace OneConfig.Services.ConfigurationReaders
{
    [IdentifyByString("this file")]
    public class AppSettingsReader : IConfigurationReader
    {
        public string GetSingleValue(string key)
        {
#if NETSTD
            return null; //not supported
#else
            return ConfigurationManager.AppSettings[key];
#endif
        }

        public override string ToString()
        {
            return "Application Configuration Settings Reader";
        }
    }
}
