using OneConfig.Models;
using OneConfig.Models.Exceptions;
using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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
            if (ConfigVariableResolver.HasUnresolvedVariables(text))
                return new ReaderLoadResult(new UnresolvedReader(text), text);

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

        public static IEnumerable<string> GetSourceKeysFromAppSettings()
        {
            yield return ConfigurationManager.AppSettings["OneConfig_Source"];

            int index = 2;
            while (true)
            {
                var readerText = ConfigurationManager.AppSettings[$"OneConfig_Source{index}"];
                if (readerText != null)
                {
                    yield return readerText;
                    index++;
                }
                else
                    break;
            }
        }

        public static IEnumerable<ReaderLoadResult> FromAppSettings(bool wrapWithInMemoryReader)
        {
            var keys = GetSourceKeysFromAppSettings().ToArray();
            if (keys.Length == 0)
                throw new Exception("No configuration sources were found. Please provide a key in your application config named OneConfig_Source.");

            foreach(var key in keys)            
                yield return FromString(key, wrapWithInMemoryReader);     
        }

        /// <summary>
        /// Replaces any UnresolvedReader instances with resolved versions
        /// </summary>
        /// <param name="readers"></param>
        /// <param name="wrapWithInMemoryReader"></param>
        /// <returns>The result of any resolved readers</returns>
        public static IEnumerable<ReaderLoadResult> ResolveReadersWithVariables(List<IConfigurationReader> readers, bool wrapWithInMemoryReader)
        {
            var provider = new ConfigurationProvider(readers);
            var unresolvedReaders = readers.OfType<UnresolvedReader>().ToArray();

            var unresolvedReaderCount = unresolvedReaders.Length;

            while (true)
            {
                foreach (var unresolved in unresolvedReaders)
                {
                    var resolvedText = ConfigVariableResolver.Resolve(provider, unresolved.LoadString);
                    if (!ConfigVariableResolver.HasUnresolvedVariables(resolvedText))
                    {
                        var result = FromString(resolvedText, wrapWithInMemoryReader);
                        if (result.Reader != null)
                        {
                            var replaceIndex = readers.IndexOf(unresolved);
                            readers[replaceIndex] = result.Reader;
                        }

                        yield return result;
                    }
                }

                unresolvedReaders = readers.OfType<UnresolvedReader>().ToArray();

                if (unresolvedReaders.Length == 0 || unresolvedReaders.Length == unresolvedReaderCount)
                    break;
            }
        }
    }
}
