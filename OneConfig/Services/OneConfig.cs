using OneConfig.Models;
using OneConfig.Models.Exceptions;
using OneConfig.Services;
using OneConfig.Services.ConfigurationProvider;
using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;
using System.Collections.Generic;

namespace OneConfig
{
    public static class OneConfig
    {
        private static IConfigurationProvider _provider;
        private static RuntimeConfigurationReader _runtimeConfigReader;
     
        public static ConfigurationException[] ReaderLoadErrors { get; private set; }
      
        public static IConfigurationProvider GetProvider()
        {
            if(_provider == null)
            {
                List<ConfigurationException> loadErrors = new List<ConfigurationException>();
                List<IConfigurationReader> readers = new List<IConfigurationReader>();

                foreach(var result in ReaderFactory.FromAppSettings(wrapWithInMemoryReader: true))
                {
                    if (result.Error != null)
                        loadErrors.Add(result.Error);
                    else if (result.Reader != null)
                        readers.Add(result.Reader);
                }

                ReaderLoadErrors = loadErrors.ToArray();
                _provider = new ConfigurationProvider(readers);

            }
            return _provider;
        }

        public static RuntimeConfigurationReader GetRuntimeReader()
        {
            if(_runtimeConfigReader == null)
            {
                _runtimeConfigReader = new RuntimeConfigurationReader();
                GetProvider().AddReader(_runtimeConfigReader);
            }

            return _runtimeConfigReader;
        }

        public static void AddReader(IConfigurationReader newReader)
        {
            var provider = GetProvider();
            provider.AddReader(newReader);
        }

        public static void SetValue(string key, string value)
        {
            var runtimeReader = GetRuntimeReader();
            runtimeReader.SetConfiguration(key, value);
        }

        public static void ResetToDefault(string key)
        {
            var runtimeReader = GetRuntimeReader();
            runtimeReader.RemoveKey(key);
        }

        public static string GetValue(string key)
        {
            var provider = GetProvider();
            var value = provider.GetValue(key);

            var resolver = new ConfigVariableResolver();
            return resolver.Resolve(provider, value.Text);
        }

        public static ConfigSourceDescription GetValueSource(string key)
        {
            var provider = GetProvider();
            var value = provider.GetValue(key);
            return new ConfigSourceDescription(value);
        }
    }
}
