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
            return ConfigurationManager.AppSettings[key];
        }
    }
}
