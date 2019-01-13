using OneConfig.Models.Exceptions;
using OneConfig.Services.Interfaces;

namespace OneConfig.Models
{
    public class ReaderLoadResult
    {
        public IConfigurationReader Reader { get; }
        public ConfigurationException Error { get; }
        public string LoadString { get; }
        
        public ReaderLoadResult(IConfigurationReader reader, string loadString)
        {
            Reader = reader;
            LoadString = loadString;
        }

        public ReaderLoadResult(ConfigurationException error, string loadString)
        {
            Error = error;
            LoadString = loadString;
        }        
    }
}
