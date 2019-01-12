using OneConfig.Services.Interfaces;
using System.Configuration;

namespace OneConfig.Services.ConfigurationReaders
{
    public class AppSettingsReader : IConfigurationReader
    {
        public string GetSingleValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
