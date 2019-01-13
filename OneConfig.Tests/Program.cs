using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneConfig.Tests
{
    class Program
    {
        public static void Main(string[] args)
        {
            var cmdValue1 = OneConfig.GetValue("commandLineKey1");
            var cmdValue2 = OneConfig.GetValue("commandLineKey2");

            Assert.AreEqual("command line value", cmdValue1);
            Assert.AreEqual("valuefromcmd2", cmdValue2);            
        }
    }
}
