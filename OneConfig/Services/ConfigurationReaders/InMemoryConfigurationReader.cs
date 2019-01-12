using OneConfig.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneConfig.Services.ConfigurationReaders
{
    /// <summary>
    /// Remembers values in memory so they are only read from their source once
    /// </summary>
    class InMemoryConfigurationReader : IConfigurationReader
    {
        private Dictionary<string, string> _configValues = new Dictionary<string, string>();
        private IConfigurationReader _actualReader;
        private bool _triedPreload;

        public InMemoryConfigurationReader(IConfigurationReader actualReader)
        {
            _actualReader = actualReader;
        }

        private void TryPreloadValues()
        {
            var multiReader = _actualReader as IMultiConfigurationReader;
            if (multiReader != null)
                _configValues = multiReader.GetAllValues() ?? new Dictionary<string, string>();

            _triedPreload = true;
        }

        public string GetSingleValue(string key)
        {
            if (!_triedPreload)
                TryPreloadValues();

            string result;

            lock (this)
            {                
                if (!_configValues.TryGetValue(key, out result))
                {
                    result = _actualReader.GetSingleValue(key);
                    _configValues.Add(key, result);
                }
            }

            return result;
        }
    }
}
