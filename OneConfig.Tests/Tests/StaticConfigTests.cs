using NUnit.Framework;
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
        [TestCase("SampleKey","SampleValue")]
        [TestCase("SampleXMLKey","SampleXMLValue")]
        [TestCase("SampleKeyToOverride", "Overridden Value")]
        [TestCase("ConfigWithVariables", "Resolved Value")]

        public void CanReadConfigValue(string key, object expectedValue)
        {
            Assert.AreEqual(expectedValue, OneConfig.GetValue(key));
        }
    }
}
