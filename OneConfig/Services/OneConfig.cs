using OneConfig.Services;
using OneConfig.Services.ConfigurationProvider;
using OneConfig.Services.Interfaces;

namespace OneConfig
{
    public static class OneConfig
    {
        private static IConfigurationProvider _provider;
        
        public static IConfigurationProvider GetProvider()
        {
            _provider = _provider ?? new ConfigurationProvider(ReaderFactory.FromAppSettings(wrapWithInMemoryReader:true));            
            return _provider;
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
