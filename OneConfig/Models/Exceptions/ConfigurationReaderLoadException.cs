using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneConfig.Models.Exceptions
{
    public class ConfigurationReaderLoadException : Exception
    {
        public ConfigurationReaderLoadException(string providerText) : base($"Unable to determine the provider that matches the string: {providerText}")
        {
        }
    }
}
