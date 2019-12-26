using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneConfig.Models.Exceptions
{
    public class ConfigurationValueCastException : Exception
    {

        public ConfigurationValueCastException(string key, string value, Type targetType)
        : base($"Unable to cast Configuration Value {key}={value} to type {targetType.Name}")
        {
        }
    }
}
