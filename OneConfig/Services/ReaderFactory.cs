﻿using OneConfig.Models;
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

        public static ReaderLoadResult FromString(string text, bool wrapWithInMemoryReader=true)
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
            var firstSource = ConfigurationManager.AppSettings["OneConfig_Source"];
            if (!String.IsNullOrEmpty(firstSource))
            {
                yield return firstSource;

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
        }

        private static string[] GetStandardSourceKeys()
        {
            return new string[] { "this file", "windows environment", "command line" };
        }

        public static IEnumerable<ReaderLoadResult> FromAppSettings(bool wrapWithInMemoryReader=true)
        {
            var keys = GetSourceKeysFromAppSettings().ToArray();
            if (keys.Length == 0)
                keys = GetStandardSourceKeys();

            foreach(var key in keys)            
                yield return FromString(key, wrapWithInMemoryReader);     
        }

    }
}
