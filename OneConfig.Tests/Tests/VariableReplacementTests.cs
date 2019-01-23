using NUnit.Framework;
using OneConfig.Models.Exceptions;
using OneConfig.Services;
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
            result = ConfigVariableResolver.Resolve(provider, result);

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
            result = ConfigVariableResolver.Resolve(provider, result);

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
        
            Assert.Throws<CyclicVariableDependencyException>(() => ConfigVariableResolver.Resolve(provider, result));
        }
    }
}
