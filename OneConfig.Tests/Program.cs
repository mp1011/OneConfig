using NUnit.Framework;

namespace OneConfig.Tests
{
    class Program
    {
        public static void Main(string[] args)
        {
            var cmdValue1 = AppConfig.GetValue("commandLineKey1");
            var cmdValue2 = AppConfig.GetValue("commandLineKey2");

            Assert.AreEqual("command line value", cmdValue1);
            Assert.AreEqual("valuefromcmd2", cmdValue2);            
        }
    }
}
