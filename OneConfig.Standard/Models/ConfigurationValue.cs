using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;
using System.Collections.Generic;

namespace OneConfig.Models
{
    public class ConfigurationValue
    {
        public string Key { get; }

        public string Text => value?.Value;

        private ConfigurationValueFromReader value;

        private List<ConfigurationValueFromReader> overriddenValues = new List<ConfigurationValueFromReader>();

        public IConfigurationReader Provider => value?.Reader;

        public IEnumerable<ConfigurationValueFromReader> OverriddenValues => overriddenValues.AsReadOnly();

        public ConfigurationValue(string key)
        {
            Key = key;
        }

        public bool TryOverrideWith(IConfigurationReader reader)
        {
            if (value != null)            
                overriddenValues.Add(value);

            var valueFromReader = reader.GetSingleValue(Key);
            if (valueFromReader != null)
            {
                value = new ConfigurationValueFromReader(reader, valueFromReader);
                return true;
            }
            else
                return false;
        }
    }
}
