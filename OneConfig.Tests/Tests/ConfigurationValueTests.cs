using NUnit.Framework;
using OneConfig.Models;
using OneConfig.Services.ConfigurationReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneConfig.Tests
{
    [TestFixture]
    class ConfigurationValueTests
    {
        [TestCase]
        public void ConfigValueRemembersOverriddenSources()
        {
            var firstReader = new MockConfigurationReader();
            var secondReader = new MockConfigurationReader();

            firstReader.SetConfiguration("SampleValue", "ValueFromFirst");
            secondReader.SetConfiguration("SampleValue", "ValueFromSecond");

            var configValue = new ConfigurationValue("SampleValue");
            configValue.TryOverrideWith(firstReader);
            configValue.TryOverrideWith(secondReader);

            Assert.AreEqual("ValueFromSecond", configValue.Text);
            Assert.AreEqual(firstReader, configValue.OverriddenValues.Single().Reader);
            Assert.AreEqual(secondReader, configValue.Provider);
        }
    }
}
