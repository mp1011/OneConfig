using NUnit.Framework;
using OneConfig.Services;
using OneConfig.Services.ConfigurationReaderFactories;
using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;
using OneConfig.Tests.TestModels;
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
            var reader = ReaderFactory.FromString(text, false).Reader;
            Assert.AreEqual(expectedReaderType, reader.GetType());
        }

        [Test]
        public void CanParseReadersFromAppSettings()
        {
            var readers = ReaderFactory.FromAppSettings(false).ToArray();
            Assert.AreEqual(4, readers.Length);

            Assert.IsInstanceOf<AppSettingsReader>(readers[0].Reader);
            Assert.IsInstanceOf<XMLSectionReader>(readers[1].Reader);
        }

        [Test]
        public void FriendlyErrorGivenIfReaderHasNoParamerlessConstructor()
        {
            var factory = new StandardReaderFactory(new IConfigurationReader[] { new MissingConstructor("") });
            try
            {
                var reader = factory.TryParseReader("bad ctor");
            }
            catch (Exception e)
            {
                Assert.That(e.Message.Contains("Make sure this type has a public parameterless constructor."));
            }
        }

        [Test]
        public void MissingConfigurationDoesNotCrashProgram()
        {
            var anyConfig = OneConfig.GetValue("any");
            var error = OneConfig.ReaderLoadErrors.First();
            Assert.That(error.Message.Contains("The file does not exist"));
        }
    }
}
