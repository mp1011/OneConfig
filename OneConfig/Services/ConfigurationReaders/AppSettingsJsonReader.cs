using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneConfig.Models;
using OneConfig.Services;
using OneConfig.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OneConfig.Services.ConfigurationReaders
{
    [IdentifyByString("appsettings.json")]
    public class AppSettingsJsonReader : IConfigurationReader
    {
        private const string _fileName = "appsettings.json";
        private JObject _settingsObject;

        private JObject GetSettingsObject()
        {
            if (_settingsObject == null)
            {
                var file = new FileInfo($"{FileHelper.ApplicationDirectory}{Path.DirectorySeparatorChar}{_fileName}");
                var json = File.ReadAllText(file.FullName);
                _settingsObject = (JObject)JsonConvert.DeserializeObject(json);
            }

            return _settingsObject;
        }

        public string GetSingleValue(string key)
        {
            var settings = GetSettingsObject();
            return settings.Value<string>(key);
        }
    }
}
