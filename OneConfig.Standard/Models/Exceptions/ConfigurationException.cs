using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneConfig.Models.Exceptions
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string text) : base(text) { }

        public ConfigurationException(string text, Exception innerException) : base(text, innerException) { }
    }
}
