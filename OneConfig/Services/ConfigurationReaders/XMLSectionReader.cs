using OneConfig.Models;
using OneConfig.Services.Interfaces;
using System;
using System.Xml;
using System.Collections.Generic;
using OneConfig.Models.Exceptions;

namespace OneConfig.Services.ConfigurationReaders
{
    /// <summary>
    /// Reads elements from an xml node
    /// </summary>
    public class XMLSectionReader : IConfigurationReader
    {
        private XmlNode _settingsContainerNode;
        private string _filePath;

        public XMLSectionReader(string filePath, string sectionXPath) 
        {
            _filePath = filePath;

            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(filePath);
            }
            catch(Exception e)
            {
                throw new ConfigurationException($"Error loading xml from {filePath}. Error: {e.Message}", e);
            }

            try
            {
                _settingsContainerNode = document.SelectSingleNode(sectionXPath);
            }
            catch (Exception e)
            {
                throw new ConfigurationException($"Unable to read configuration because the xpath \"{sectionXPath}\" could not be evaluated. Error: {e.Message}", e);
            }

            if (_settingsContainerNode == null)
                throw new ConfigurationException($"Unable to read configuration because the xpath {sectionXPath} was not found in the given xml document");

        }

        public XMLSectionReader(XmlDocument document, string sectionXPath)
        {
            _settingsContainerNode = document.SelectSingleNode(sectionXPath);
            if (_settingsContainerNode == null)
                throw new ConfigurationException($"Unable to read configuration because the xpath {sectionXPath} was not found in the given xml document");
        }

        public string GetSingleValue(string key)
        {
            var node = _settingsContainerNode.SelectSingleNode($"//add[@key='{key}']");
            if (node == null)
                return null;
            else
                return node.Attributes["value"]?.Value;
        }

        public override string ToString()
        {
            return $"XML Configuration Reader ({_filePath})";            
        }
    }
}
