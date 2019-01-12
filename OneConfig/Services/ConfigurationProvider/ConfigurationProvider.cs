using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneConfig.Models;
using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;

namespace OneConfig.Services.ConfigurationProvider
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private  IConfigurationReader[] _configReaders;

        public ConfigurationProvider(IEnumerable<IConfigurationReader> readers)
        {
            _configReaders = readers.ToArray();
        }

        public ConfigurationValue GetValue(string key)
        {
            ConfigurationValue ret = new ConfigurationValue(key);

            foreach (var reader in _configReaders)
            {
                ret.TryOverrideWith(reader);
            }
            
            return ret;
        }
    }
}
