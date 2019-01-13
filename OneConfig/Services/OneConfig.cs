using OneConfig.Services;
using OneConfig.Services.ConfigurationProvider;
using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;

namespace OneConfig
{
    public static class OneConfig
    {
        private static IConfigurationProvider _provider;
        private static RuntimeConfigurationReader _runtimeConfigReader;

        public static IConfigurationProvider GetProvider()
        {
            _provider = _provider ?? new ConfigurationProvider(ReaderFactory.FromAppSettings(wrapWithInMemoryReader:true));            
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
    }
}
