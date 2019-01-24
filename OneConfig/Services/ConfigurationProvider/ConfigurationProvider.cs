using OneConfig.Models;
using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OneConfig.Services
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private List<IConfigurationReader> _configReaders;
    
        public ConfigurationProvider(IEnumerable<IConfigurationReader> readers)
        {
            _configReaders = readers.ToList();
        }
      
        public void AddReader(IConfigurationReader reader)
        {
            lock (this)
            {
                _configReaders.Add(reader);
            }
        }

        public void TryResolveReaders()
        {
            foreach(var unresolvedReader in _configReaders.OfType<UnresolvedReader>())
                unresolvedReader.TryResolve(this);
        }

        public ConfigurationValue GetValue(string key)
        {
            ConfigurationValue ret = new ConfigurationValue(key);

            lock (this)
            {
                foreach (var reader in _configReaders)
                {
                    ret.TryOverrideWith(reader);
                }
            }

            return ret;
        }
    }
}
