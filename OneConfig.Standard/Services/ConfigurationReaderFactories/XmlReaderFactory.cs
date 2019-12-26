using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneConfig.Services.ConfigurationReaders;
using System.IO;
using OneConfig.Models.Exceptions;
using OneConfig.Services.Interfaces;

namespace OneConfig.Services.ConfigurationReaderFactories
{
    public class XmlReaderFactory : IConfigurationReaderFactory
    {
        public IConfigurationReader TryParseReader(string text)
        {
            var xmlExtensionIndex = text.IndexOf(".xml", StringComparison.OrdinalIgnoreCase);
            if (xmlExtensionIndex == -1)
                return null;

            var xmlFilePath = $"{FileHelper.ApplicationDirectory}\\{text.Substring(0, xmlExtensionIndex)}.xml";
            var xPath = text.Substring(xmlExtensionIndex + 4);
            if(String.IsNullOrEmpty(xPath))
                xPath = "appSettings";

            var xmlFile = new FileInfo(xmlFilePath);
            if (xmlFile.Exists && xmlFile.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                return new XMLSectionReader(xmlFile.FullName, xPath);
            else
                throw new ConfigurationException($"Unable to load configuration from {xmlFilePath}. The file does not exist");
        }
    }
}
