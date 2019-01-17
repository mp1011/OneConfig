using OneConfig.Models;
using OneConfig.Models.Exceptions;
using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace OneConfig.Services
{
    public static class ReaderFactory
    {
        private static IConfigurationReaderFactory[] factories;
     
        static ReaderFactory()
        {
            factories = DIRegistrar.GetInstances<IConfigurationReaderFactory>();
        }

        public static ReaderLoadResult FromString(string text, bool wrapWithInMemoryReader)
        {
            foreach(var factory in factories)
            {
                try
                {
                    var reader = factory.TryParseReader(text);
                    if (reader != null)
                    {
                        if (wrapWithInMemoryReader)
                            reader = new InMemoryConfigurationReader(reader);

                        return new ReaderLoadResult(reader, text);
                    }
                }
                catch(Models.Exceptions.ConfigurationException e)
                {
                    return new ReaderLoadResult(e, text);
                }
            }


            throw new Models.Exceptions.ConfigurationException($"Unable to determine the provider that matches the string: {text}");
        }

        public static IEnumerable<ReaderLoadResult> FromAppSettings(bool wrapWithInMemoryReader)
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
