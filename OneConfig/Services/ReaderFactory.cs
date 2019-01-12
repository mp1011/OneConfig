using OneConfig.Models.Exceptions;
using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace OneConfig.Services
{
    public static class ReaderFactory
    {
        private static IConfigurationReaderFactory[] factories;

        public static string ApplicationDirectory { get; set; }

        static ReaderFactory()
        {
            factories = DIRegistrar.GetInstances<IConfigurationReaderFactory>();
            var assemblyLocation = new FileInfo(Assembly.GetExecutingAssembly().Location);
            ApplicationDirectory = assemblyLocation.Directory.FullName;
        }

        public static IConfigurationReader FromString(string text, bool wrapWithInMemoryReader)
        {
            foreach(var factory in factories)
            {
                var reader = factory.TryParseReader(text);
                if (reader != null)
                {
                    if (wrapWithInMemoryReader)
                        return new InMemoryConfigurationReader(reader);
                    else 
                        return reader;
                }
            }


            throw new ConfigurationReaderNotFoundException(text);
        }

        public static IEnumerable<IConfigurationReader> FromAppSettings(bool wrapWithInMemoryReader)
        {
            var readerText = ConfigurationManager.AppSettings["OneConfig_Source"];
            yield return FromString(readerText, wrapWithInMemoryReader);

            int index = 2;
            while(true)
            {
                readerText = ConfigurationManager.AppSettings[$"OneConfig_Source{index}"];
                if (readerText != null)
                {
                    yield return FromString(readerText, wrapWithInMemoryReader);
                    index++;
                }
                else
                    break;
            }           
        }
    }
}
