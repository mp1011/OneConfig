using OneConfig.Models;
using OneConfig.Models.Exceptions;

namespace OneConfig.Services.Interfaces
{
    public interface IConfigurationProvider
    {
        ConfigurationValue GetValue(string key);

        void AddReader(IConfigurationReader reader);
    }
}
