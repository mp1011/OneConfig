using OneConfig.Models;
using OneConfig.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace OneConfig.Services.ConfigurationReaders
{
    [IdentifyByString("windows environment")]
    public class WindowsEnvironmentConfigurationReader : IConfigurationReader
    {
        public string GetSingleValue(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}
