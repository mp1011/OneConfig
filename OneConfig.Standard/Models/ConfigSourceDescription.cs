using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;
using System.Linq;

namespace OneConfig.Models
{
    public class ConfigSourceDescription
    {
        public string Description { get; }

        public IConfigurationReader Reader {get;}

        public ConfigurationValueFromReader[] OverriddenValues { get; }

        public ConfigSourceDescription(ConfigurationValue value)
        {
            if (value == null || value.Provider == null)
                Description = "No reader contains this configuration key";
            else
            {
                Reader = value.Provider;
                OverriddenValues = value.OverriddenValues.ToArray();
                Description = "This value was provided by the " + Reader.ToString();
            }
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
