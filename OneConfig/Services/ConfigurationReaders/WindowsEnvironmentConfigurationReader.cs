using OneConfig.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace OneConfig.Services.ConfigurationReaders
{
    public class WindowsEnvironmentConfigurationReader : IConfigurationReader
    {
        public string GetSingleValue(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}
