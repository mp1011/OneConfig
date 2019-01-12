using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneConfig.Models
{
    public class UnableToReadConfigurationException : Exception
    {
        public UnableToReadConfigurationException(string message) : base(message) { }

        public UnableToReadConfigurationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
