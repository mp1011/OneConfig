using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;

namespace OneConfig.Services.ConfigurationReaderFactories
{
    public class StandardReaderFactory : IConfigurationReaderFactory
    {
        public IConfigurationReader TryParseReader(string text)
        {
            //fix me -
            if (text == "this file")
                return new AppSettingsReader();
            else if (text == "windows environment")
                return new WindowsEnvironmentConfigurationReader();
            else
                return null;
        }
    }
}
