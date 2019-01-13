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

                var inMemoryReader = Reader as InMemoryConfigurationReader;
                if (inMemoryReader != null)
                {
                    Description = $"This value was provided by the {inMemoryReader.ActualReader.GetType().Name} and is being cached in memory";

                    Reader = inMemoryReader.ActualReader;
                }
                else 
                    Description = $"This value was provided by the {Reader.GetType().Name}";


            }
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
