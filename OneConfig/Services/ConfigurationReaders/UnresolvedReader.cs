using OneConfig.Services.Interfaces;

namespace OneConfig.Services.ConfigurationReaders
{
    public class UnresolvedReader : IConfigurationReader
    {
        public string LoadString { get; }

        public UnresolvedReader(string loadString)
        {
            LoadString = loadString;
        }

        public string GetSingleValue(string key)
        {
            return null;
        }
    }
}
