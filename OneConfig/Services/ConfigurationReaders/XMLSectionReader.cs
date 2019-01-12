using OneConfig.Models;
using OneConfig.Services.Interfaces;
using System;
using System.Xml;
using System.Collections.Generic;

namespace OneConfig.Services.ConfigurationReaders
{
    /// <summary>
    /// Reads elements from an xml node
    /// </summary>
    public class XMLSectionReader : IConfigurationReader
    {
        private XmlNode _settingsContainerNode;

        public XMLSectionReader(string filePath, string sectionXPath) 
        {
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(filePath);
            }
            catch(Exception e)
            {
                throw new UnableToReadConfigurationException($"Error loading xml from {filePath}. Error: {e.Message}", e);
            }

            try
            {
                _settingsContainerNode = document.SelectSingleNode(sectionXPath);
            }
            catch (Exception e)
            {
                throw new UnableToReadConfigurationException($"Unable to read configuration because the xpath \"{sectionXPath}\" could not be evaluated. Error: {e.Message}", e);
            }

            if (_settingsContainerNode == null)
                throw new UnableToReadConfigurationException($"Unable to read configuration because the xpath {sectionXPath} was not found in the given xml document");

        }

        public XMLSectionReader(XmlDocument document, string sectionXPath)
        {
            _settingsContainerNode = document.SelectSingleNode(sectionXPath);
            if (_settingsContainerNode == null)
                throw new UnableToReadConfigurationException($"Unable to read configuration because the xpath {sectionXPath} was not found in the given xml document");
        }

        public string GetSingleValue(string key)
        {
            var node = _settingsContainerNode.SelectSingleNode($"//add[@key='{key}']");
            if (node == null)
                return null;
            else
                return node.Attributes["value"]?.Value;
        }
    }
}
