using OneConfig.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneConfig.Services.ConfigurationReaders
{
    /// <summary>
    /// Reader that allows its values to be set at runtime
    /// </summary>
    public class RuntimeConfigurationReader : IConfigurationReader
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

        public void RemoveKey(string key)
        {
            _values.Remove(key);
        }        
    }
}
