using OneConfig.Models.Exceptions;
using OneConfig.Services;
using OneConfig.Services.Interfaces;

namespace OneConfig.Models
{
    public class ReaderLoadResult
    {
        public IConfigurationReader Reader { get; }
        public ConfigurationException Error { get; }
        public string LoadString { get; }

        public bool HasUnresolvedVariables => ConfigVariableResolver.HasUnresolvedVariables(LoadString);

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
