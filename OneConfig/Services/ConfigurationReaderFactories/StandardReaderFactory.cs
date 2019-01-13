using OneConfig.Models;
using OneConfig.Models.Exceptions;
using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;
using System;
using System.Linq;

namespace OneConfig.Services.ConfigurationReaderFactories
{
    public class StandardReaderFactory : IConfigurationReaderFactory
    {
        private Type[] _allReaderTypes;

        public StandardReaderFactory(IConfigurationReader[] allReaders)
        {
            _allReaderTypes = allReaders.Select(p => p.GetType()).ToArray();
        }

        public IConfigurationReader TryParseReader(string text)
        {
            foreach(var readerType in _allReaderTypes)
            {
                var nameAttr = readerType.GetCustomAttributes(true)
                    .OfType<IdentifyByStringAttribute>()
                    .SingleOrDefault();

                if (nameAttr != null && nameAttr.Name == text)
                    return CreateReader(readerType);
            }

            return null;
        }

        private IConfigurationReader CreateReader(Type readerType)
        {
            try
            {
                IConfigurationReader reader = (IConfigurationReader)Activator.CreateInstance(readerType);
                return reader;
            }
            catch(Exception e)
            {
                throw new ConfigurationReaderLoadException($"Unable to create a reader of type {readerType.FullName}. Make sure this type has a public parameterless constructor.");
            }
        }
    }
}
