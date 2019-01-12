using NUnit.Framework;
using OneConfig.Models.Exceptions;
using OneConfig.Services;
using OneConfig.Services.ConfigurationProvider;
using OneConfig.Services.ConfigurationReaders;
using OneConfig.Services.Interfaces;

namespace OneConfig.Tests
{
    [TestFixture]
    class VariableReplacementTests
    {
        [Test]
        public void CanResolveVariablesWithinConfiguration()
        {
            var mockReader = new MockConfigurationReader();
            mockReader.SetConfiguration("VarA", "ValueA");
            mockReader.SetConfiguration("VarB", "#{VarA} + #{VarC}");
            mockReader.SetConfiguration("VarC", "ValueC");

            var provider = new ConfigurationProvider(new IConfigurationReader[] { mockReader });
            var result = mockReader.GetSingleValue("VarB");
            var resolver = new ConfigVariableResolver();
            result = resolver.Resolve(provider, result);

            Assert.AreEqual("ValueA + ValueC", result);
        }

        [Test]
        public void CanResolveMultiLevelVariablesWithinConfiguration()
        {
            var mockReader = new MockConfigurationReader();
            mockReader.SetConfiguration("VarA", "(#{VarB})");
            mockReader.SetConfiguration("VarB", "#{VarC} + #{VarC}");
            mockReader.SetConfiguration("VarC", "ValueC");

            var provider = new ConfigurationProvider(new IConfigurationReader[] { mockReader });
            var result = mockReader.GetSingleValue("VarA");
            var resolver = new ConfigVariableResolver();
            result = resolver.Resolve(provider, result);

            Assert.AreEqual("(ValueC + ValueC)", result);
        }

        [Test]
        public void CyclicDepedenciesCanBeIdentified()
        {
            var mockReader = new MockConfigurationReader();
            mockReader.SetConfiguration("VarA", "#{VarB}");
            mockReader.SetConfiguration("VarB", "#{VarC}");
            mockReader.SetConfiguration("VarC", "#{VarA}");

            var provider = new ConfigurationProvider(new IConfigurationReader[] { mockReader });
            var result = mockReader.GetSingleValue("VarB");
            var resolver = new ConfigVariableResolver();

            Assert.Throws<CyclicVariableDependencyException>(() => resolver.Resolve(provider, result));
        }
    }
}
