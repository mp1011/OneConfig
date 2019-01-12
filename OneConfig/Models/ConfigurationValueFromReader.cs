using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;

namespace OneConfig.Models
{
    public class ConfigurationValueFromReader
    {
        public IConfigurationReader Reader { get; }

        public string Value { get; }

        public ConfigurationValueFromReader(IConfigurationReader reader, string value)
        {
            Reader = reader;
            Value = value;
        }
    }
}
