﻿using OneConfig.Services.Interfaces;
using System.Collections.Generic;

namespace OneConfig.Services.ConfigurationReaders
{
    class MockConfigurationReader : IConfigurationReader
    {
        private Dictionary<string, string> _values = new Dictionary<string, string>();

        public string GetSingleValue(string key)
        {
            string ret = null;
            _values.TryGetValue(key, out ret);
            return ret;
        }

        public void SetConfiguration(string key, string value)
        {
            _values[key] = value;
        }
    }
}
