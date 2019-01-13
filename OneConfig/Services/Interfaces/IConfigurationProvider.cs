using OneConfig.Models;

namespace OneConfig.Services.Interfaces
{
    public interface IConfigurationProvider
    {
        ConfigurationValue GetValue(string key);

        void AddReader(IConfigurationReader reader);
    }
}
