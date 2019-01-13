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
            Assert.AreEqual(expectedValue, OneConfig.GetValue(key));
        }


        [Test]
        public void CanInjectNewReaderAtRuntime()
        {
            var newReader = new MockConfigurationReader();
            newReader.SetConfiguration("AddedAt", "Runtime");

            Assert.AreEqual(null, OneConfig.GetValue("AddedAt"));
            OneConfig.AddReader(newReader);
            Assert.AreEqual("Runtime", OneConfig.GetValue("AddedAt"));
        }

        [Test]
        public void CanSetConfigValuesAtRuntime()
        {
            var oldValue = OneConfig.GetValue("SampleKey");

            try
            {
                OneConfig.SetValue("SampleKey", "Changed at runtime");
                Assert.AreEqual("Changed at runtime", OneConfig.GetValue("SampleKey"));
            }
            finally
            {
                OneConfig.ResetToDefault("SampleKey");
                Assert.AreEqual(oldValue, OneConfig.GetValue("SampleKey"));
            }
        }

        [TestCase("doesnt exist", "No reader contains this configuration key")]
        [TestCase("SampleKey", "This value was provided by the AppSettingsReader and is being cached in memory")]
        [TestCase("SampleXMLKey", "This value was provided by the XMLSectionReader and is being cached in memory")]
        public void CanGetSourceOfConfigValue(string key, string expectedSource)
        {
            Assert.AreEqual(expectedSource, OneConfig.GetValueSource(key).Description);
        }
    }
}
