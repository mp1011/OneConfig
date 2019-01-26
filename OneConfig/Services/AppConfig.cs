using OneConfig.Models;
using OneConfig.Models.Exceptions;
using OneConfig.Services;
using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace OneConfig
{
    public static class AppConfig
    {       
        private static IConfigurationProvider _provider;
        private static RuntimeConfigurationReader _runtimeConfigReader;
     
        public static ConfigurationException[] ReaderLoadErrors { get; private set; }
      
        static AppConfig()
        {
            Reset();
        }

        internal static IConfigurationProvider GetProvider()
        {
            return _provider;
        }

        private static RuntimeConfigurationReader GetRuntimeReader()
        {
            if(_runtimeConfigReader == null)
            {
                _runtimeConfigReader = new RuntimeConfigurationReader();
                _provider.AddReader(_runtimeConfigReader);
            }

            return _runtimeConfigReader;
        }

        public static void AddReader(IConfigurationReader newReader)
        {
            _provider.AddReader(newReader);
            _provider.OnValueChanged();
        }

        public static void SetValue(string key, string value)
        {
            var runtimeReader = GetRuntimeReader();
            runtimeReader.SetConfiguration(key, value);
            _provider.OnValueChanged();
        }
         
        public static void ResetToDefault(string key)
        {
            var runtimeReader = GetRuntimeReader();
            runtimeReader.RemoveKey(key);
        }
        
        /// <summary>
        /// Returns the given configuration value, with any #{ } variables resolved,
        /// from the highest indexed configuration source that contains the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="required"></param>
        /// <returns></returns>
        public static string GetValue(string key, bool required=false)
        {
            var value = _provider.GetValue(key);
            
            var result = ConfigVariableResolver.Resolve(_provider, value.Text);

            if(required && string.IsNullOrEmpty(result))
                throw new RequiredValueNotFoundException(key);

            return result;
        }


        /// <summary>
        /// Returns the given configuration value as an integer. Returns 0 if the value was not found
        /// or is not a number.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetInteger(string key)
        {
            var textValue = GetValue(key);
            int ret;
            if (int.TryParse(textValue, out ret))
                return ret;
            else
                return 0;
        }

        /// <summary>
        /// Interprets the given configuration value as a directory.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="create">Optionally creates the directory is not found</param>
        /// <returns></returns>
        public static DirectoryInfo GetDirectory(string key, FileNotFoundOption directoryNotFoundOption)
        {
            var directoryName = GetValue(key);

            var directory = new DirectoryInfo(FileHelper.ToAbsolutePath(directoryName));

            if(!directory.Exists)
            {
                switch (directoryNotFoundOption)
                {
                    case FileNotFoundOption.Create:
                        directory.Create();

                        //we have to actually remake the object or else the "Exists" property will be false
                        directory = new DirectoryInfo(FileHelper.ToAbsolutePath(directoryName));

                        break;
                    case FileNotFoundOption.ThrowError:
                        throw new ConfigurationException($"The directory {directory.FullName} for configuration {key} does not exist.");
                }
            }

            return directory;
        }

        /// <summary>
        /// Interprets the given configuration value as a file.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="create">Optionally creates the directory is not found</param>
        /// <returns></returns>
        public static FileInfo GetFile(string key, FileNotFoundOption fileNotFoundOption)
        {
            var textValue = GetValue(key);
            var file = new FileInfo(textValue);

            if (!file.Exists)
            {
                switch (fileNotFoundOption)
                {
                    case FileNotFoundOption.Create:

                        try
                        {
                            if (!file.Directory.Exists)
                                file.Directory.Create();
                            file.Create();
                        }
                        catch(Exception e)
                        {
                            throw new ConfigurationException($"Failed to create the file {textValue} for configuration {key}.", e);
                        }
                        break;
                    case FileNotFoundOption.ThrowError:
                        throw new ConfigurationException($"The file {textValue} for configuration {key} does not exist.");
                }
            }

            return file;
        }

        public static ConfigSourceDescription GetValueSource(string key)
        {
            var value = _provider.GetValue(key);
            return new ConfigSourceDescription(value);
        }

        public static void Reset()
        {
            _provider = null;
            _runtimeConfigReader = null;
            ReaderLoadErrors = null;

            List<ConfigurationException> loadErrors = new List<ConfigurationException>();
            List<IConfigurationReader> readers = new List<IConfigurationReader>();

            foreach (var result in ReaderFactory.FromAppSettings())
            {
                if (result.Error != null)
                    loadErrors.Add(result.Error);
                else if (result.Reader != null)
                    readers.Add(result.Reader);
            }
            
            ReaderLoadErrors = loadErrors.ToArray();
            _provider = new ConfigurationProvider(readers);
        }      
    }
}
