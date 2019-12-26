using System;
using OneConfig.Services.Interfaces;

namespace OneConfig.Services.ConfigurationReaders
{
    public class UnresolvedReader : IConfigurationReader, IResettable
    {
        public string LoadString { get; }

        private IConfigurationReader _resolvedReader;

        public UnresolvedReader(string loadString)
        {
            LoadString = loadString;
        }
        
        public string GetSingleValue(string key)
        {
            if (_resolvedReader != null)
                return _resolvedReader.GetSingleValue(key);

            return null;
        }

        public override string ToString()
        {
            if (_resolvedReader == null)
                return "Unknown Reader";
            else
                return _resolvedReader.ToString();
        }

        void IResettable.Reset()
        {
            var provider = AppConfig.GetProvider();
            _resolvedReader = null;

            var resolvedText = ConfigVariableResolver.Resolve(provider, LoadString);
            if (!String.IsNullOrEmpty(resolvedText) && !ConfigVariableResolver.HasUnresolvedVariables(resolvedText))
            {
                var resolveResult = ReaderFactory.FromString(resolvedText);
                if (resolveResult.Reader != null)
                    _resolvedReader = resolveResult.Reader;
           }
            
        }
    }
}
