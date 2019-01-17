using NUnit.Framework;
using OneConfig.Models;
using OneConfig.Models.Exceptions;
using OneConfig.Services;
using OneConfig.Services.ConfigurationReaders;
using System;
using System.IO;

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
            var dbReader = new DatabaseConfigurationReader(AppConfig.GetValue("DatabaseConnectionString"));
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

        [Test]
        public void CanReadConfigurationFromStringArray()
        {
            var args = new string[] { "/Key1:Value1", "/Key2:\"Value Two\"" };
            var reader = new StringArrayConfigurationReader(args);

            Assert.AreEqual("Value1", reader.GetSingleValue("Key1"));
            Assert.AreEqual("Value Two", reader.GetSingleValue("Key2"));
        }

        [Test]
        public void ValueCanBeRequired()
        {
            Assert.Throws<RequiredValueNotFoundException>(() => AppConfig.GetValue("no such value exists", required: true));
        }

        [Test]
        public void CanParseValueAsInteger()
        {
            Assert.AreEqual(12345, AppConfig.GetInteger("ThisIsANumber"));
            Assert.AreEqual(0, AppConfig.GetInteger("ThisIsAPath"));
        }

        [Test]
        public void CanParseValueAsDirectory()
        {
            FileHelper.ApplicationDirectory = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);

            var path = AppConfig.GetDirectory("ThisIsAPath", FileNotFoundOption.DoNothing);
            Assert.That(path.Exists);

            var newPath = AppConfig.GetDirectory("ThisIsANewPath", FileNotFoundOption.DoNothing);
            if(newPath.Exists)
                newPath.Delete();

            Assert.Throws<ConfigurationException>(() => AppConfig.GetDirectory("ThisIsANewPath", FileNotFoundOption.ThrowError));

            newPath = AppConfig.GetDirectory("ThisIsANewPath", FileNotFoundOption.Create);
            Assert.That(newPath.Exists);
        }

        [Test]
        public void CanParseValueAsAbsoluteDirectory()
        {
            FileHelper.ApplicationDirectory = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);

            var path = AppConfig.GetDirectory("ThisIsAPath", FileNotFoundOption.DoNothing);
            Assert.That(path.Exists);


            var mockReader = new MockConfigurationReader();
            mockReader.SetConfiguration("AbsoluteDir", path.FullName);

            var absolutePath = AppConfig.GetDirectory("AbsoluteDir", FileNotFoundOption.ThrowError);

            AppConfig.AddReader(mockReader);
        }

        [TearDown]
        public void Teardown()
        {
            AppConfig.Reset();
        }
    }
}
