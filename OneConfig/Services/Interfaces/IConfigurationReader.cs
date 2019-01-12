using System.Collections.Generic;

namespace OneConfig.Services.Interfaces
{
    public interface IConfigurationReader
    {
        string GetSingleValue(string key);
    }

    public interface IMultiConfigurationReader : IConfigurationReader
    {
        Dictionary<string, string> GetAllValues();
    }
}
