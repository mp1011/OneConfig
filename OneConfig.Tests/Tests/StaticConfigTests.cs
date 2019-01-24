using NUnit.Framework;
using OneConfig.Services.ConfigurationReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneConfig.Tests
{
    [TestFixture]
    class StaticConfigTests
    {
        [TestCase("SampleKey", "SampleValue")]
        [TestCase("SampleXMLKey", "SampleXMLValue")]
        [TestCase("SampleKeyToOverride", "Overridden Value")]
        [TestCase("ConfigWithVariables", "Resolved Value")]

        public void CanReadConfigValue(string key, object expectedValue)
        {
            Assert.AreEqual(expectedValue, AppConfig.GetValue(key));
        }


        [Test]
        public void CanInjectNewReaderAtRuntime()
        {
            var newReader = new MockConfigurationReader();
            newReader.SetConfiguration("AddedAt", "Runtime");

            Assert.AreEqual(null, AppConfig.GetValue("AddedAt"));
            AppConfig.AddReader(newReader);
            Assert.AreEqual("Runtime", AppConfig.GetValue("AddedAt"));
        }

        [Test]
        public void CanSetConfigValuesAtRuntime()
        {
            var oldValue = AppConfig.GetValue("SampleKey");

            try
            {
                AppConfig.SetValue("SampleKey", "Changed at runtime");
                Assert.AreEqual("Changed at runtime", AppConfig.GetValue("SampleKey"));
            }
            finally
            {
                AppConfig.ResetToDefault("SampleKey");
                Assert.AreEqual(oldValue, AppConfig.GetValue("SampleKey"));
            }
        }

        [TestCase("doesnt exist", "No reader contains this configuration key")]
        [TestCase("SampleKey", "This value was provided by the Application Configuration Settings Reader")]
        [TestCase("SampleXMLKey", "This value was provided by the XML Configuration Reader")]
        public void CanGetSourceOfConfigValue(string key, string expectedSource)
        {
            var description = AppConfig.GetValueSource(key).Description;
            Assert.That(description.StartsWith(expectedSource));
        }
    }
}
