using NUnit.Framework;
using OneConfig.Models;
using OneConfig.Services;
using OneConfig.Services.ConfigurationReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneConfig.Tests
{
    [TestFixture]
    class ErrorHandlingTests
    {
        [Category("Long Tests")]
        [TestCase(@"Data Source=notarealserver\SQLExpress;Initial Catalog=MyDatabase;Integrated Security=True")]
        [TestCase(@"Data Source=localhost\SQLExpress;Initial Catalog=Not a Real Database;Integrated Security=True")]
        [TestCase(@"Data Source=localhost\SQLExpress;Initial Catalog=Not a Real Database;User ID=wrong;Password=wrong")]
        [TestCase(@"Tests\SampleXML\bad.xml//mySection")]
        public void CanHandleBadConfiguration(string badConnectionString)
        {
            Assert.Throws<UnableToReadConfigurationException>(() =>
            {
                var reader = ReaderFactory.FromString(badConnectionString,false);
                var value = reader.GetSingleValue("dummy");
            });
        }       
    }
}
