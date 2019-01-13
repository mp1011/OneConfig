using OneConfig.Models;
using OneConfig.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace OneConfig.Services.ConfigurationReaders
{

    [IdentifyByString("command line")]
    public class CommandLineConfigurationReader : StringArrayConfigurationReader
    {
        public CommandLineConfigurationReader() : base(Environment.GetCommandLineArgs()) { }
    }

    public class StringArrayConfigurationReader : IMultiConfigurationReader
    {
        private string[] _args;

        /// <summary>
        /// Parses configuration elements from an array in the form /Name:Value or /Name:"Value"
        /// </summary>
        /// <param name="args"></param>
        public StringArrayConfigurationReader(IEnumerable<string> args)
        {
            _args = args.ToArray();
        }

        public Dictionary<string, string> GetAllValues()
        {
            Dictionary<string, string> nameValues = new Dictionary<string, string>();
            foreach (var arg in _args)
            {
                var splitIndex = arg.IndexOf(':');
                if (splitIndex > 0)
                {
                    var name = arg.Substring(0, splitIndex).TrimStart('/').Trim();
                    var value = arg.Substring(splitIndex + 1).Replace("\"", "").Trim();
                    nameValues.Add(name, value);
                }
                else
                {
                    //a parameter without a value (a flag)
                    nameValues.Add(arg.TrimStart('/'), null);
                }
            }

            return nameValues;
        }

        public string GetSingleValue(string key)
        {
            //should not hit here but just in case
            var allArgs = GetAllValues();
            string ret = null;
            allArgs.TryGetValue(key, out ret);
            return ret;            
        }
    }
}
