using NUnit.Framework;
using OneConfig.Services;
using OneConfig.Services.ConfigurationReaders;
using System;
using System.Linq;

namespace OneConfig.Tests
{
    [TestFixture]
    class ReaderFactoryTests
    {
        [TestCase("this file", typeof(AppSettingsReader))]
        [TestCase(@"Tests\SampleXML\sample.xml//mySection", typeof(XMLSectionReader))]
        [TestCase(@"Data Source=localhost\SQLExpress;Initial Catalog=MyDatabase;Integrated Security=True", typeof(DatabaseConfigurationReader))]
        public void CanParseReaderFromText(string text, Type expectedReaderType)
        {
            ReaderFactory.ApplicationDirectory = TestContext.CurrentContext.TestDirectory;
            var reader = ReaderFactory.FromString(text,false);
            Assert.AreEqual(expectedReaderType, reader.GetType());
        }

        [Test]
        public void CanParseReadersFromAppSettings()
        {
            var readers = ReaderFactory.FromAppSettings(false).ToArray();
            Assert.AreEqual(2, readers.Length);

            Assert.IsInstanceOf<AppSettingsReader>(readers[0]);
            Assert.IsInstanceOf<XMLSectionReader>(readers[1]);
        }
    }
}
