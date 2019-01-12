using NUnit.Framework;
using OneConfig.Services.ConfigurationReaders;
using System;

namespace OneConfig.Tests
{
    [TestFixture]
    class ConfigurationReaderTests
    {

        [TestCase("SampleKey", "SampleValue")]
        [TestCase("DoesntExist", null)]
        public void CanReadConfigurationFromAppSettings(string key, string expectedValue)
        {
            var appSettingsReader = new AppSettingsReader();
            var value = appSettingsReader.GetSingleValue(key);
            Assert.AreEqual(expectedValue, value);
        }


        [TestCase("SampleXMLKey", "SampleXMLValue")]
        [TestCase("DoesntExist", null)]
        public void CanReadConfigurationFromXMLFile(string key, string expectedValue)
        {
            var xmlReader = new XMLSectionReader(TestContext.CurrentContext.TestDirectory + @"\Tests\SampleXML\sample.xml", "//mySection");
            var value = xmlReader.GetSingleValue(key);
            Assert.AreEqual(expectedValue, value);
        }

        [TestCase("SampleDBKey", "SampleDBValue")]
        [TestCase("DoesntExist", null)]
        public void CanReadConfigurationFromDatabase(string key, string expectedValue)
        {
            var dbReader = new DatabaseConfigurationReader(OneConfig.GetValue("DatabaseConnectionString"));
            var value = dbReader.GetSingleValue(key);
            Assert.AreEqual(expectedValue, value);
        }

        [Test]
        public void CanReadConfigurationFromWindowsEnvironment()
        {
            var reader = new WindowsEnvironmentConfigurationReader();
            var value = reader.GetSingleValue("PATH");
            Assert.IsFalse(String.IsNullOrEmpty(value as string));
        }
    }
}
