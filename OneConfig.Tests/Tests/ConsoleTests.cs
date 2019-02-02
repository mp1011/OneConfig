using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace OneConfig.Tests.Tests
{
    [TestFixture]
    class ConsoleTests
    {

        private void DoConsoleTest(Dictionary<string,string> appConfigValues, string[] args, params Action<string>[] assertions)
        {
            WriteConsoleAppConfig(appConfigValues);

            using (var process = GetConsoleProcess(args))
            {
                int assertionIndex = 0;

                process.OutputDataReceived += (sender, output) =>
                {
                    if(output.Data != null)
                        assertions[assertionIndex++].Invoke(output.Data);
                };
           
                try
                {
                    process.Start();
                    process.BeginOutputReadLine();
                    process.WaitForExit(15000);
                }
                catch (Exception e)
                {
                    Assert.Fail(e.Message);
                }

                Assert.AreEqual(assertions.Length, assertionIndex, "Not all assertions were executed");
            }
      
        }

        private Process GetConsoleProcess(string[] args)
        {
            var path = $"{TestContext.CurrentContext.TestDirectory}{Path.DirectorySeparatorChar}OneConfig.ConsoleTester.exe";

            var startInfo = new ProcessStartInfo(path, String.Join(" ", args.Select(str=>$"\"{str}\"").ToArray()));
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            var process = new Process();
            process.StartInfo = startInfo;

            return process;
        }

        private void WriteConsoleAppConfig(Dictionary<string, string> appConfigValues)
        {
            var blankPath = $"{TestContext.CurrentContext.TestDirectory}{Path.DirectorySeparatorChar}App.Config.Blank";
            var configPath = $"{TestContext.CurrentContext.TestDirectory}{Path.DirectorySeparatorChar}OneConfig.ConsoleTester.exe.config";

            var doc = new XmlDocument();
            doc.Load(blankPath);

            var settingsElement = doc.DocumentElement.SelectSingleNode(@"//appSettings");
            foreach(var keyValue in appConfigValues)
            {
                var addElement = doc.CreateElement("add");
                addElement.SetAttribute("key", keyValue.Key);
                addElement.SetAttribute("value", keyValue.Value);
                settingsElement.AppendChild(addElement);
            }

            doc.Save(configPath);
        }      
        
        [Test]
        public void ConfigWithNoSourcesDefaultsToUsingItself()
        {
            DoConsoleTest(new Dictionary<string, string> { { "Sample Key", "ConfigWithNoSourcesDefaultsToUsingItself" } },
                new string[] { "?Sample Key" }, str => Assert.AreEqual("ConfigWithNoSourcesDefaultsToUsingItself", str));
        }

        [Test]
        public void ConfigSourceCanHaveVariables()
        {
            DoConsoleTest(new Dictionary<string, string>
            {
                { "OneConfig_Source", "this file" },
                { "OneConfig_Source2", "Data Source=#{source};Initial Catalog=#{database};Integrated Security=True" },
                { "source", @"localhost\SQLExpress" },
                { "database", "MyDatabase"  }
            },
                new string[] { "?SampleDBKey" }, str => Assert.AreEqual("SampleDBValue", str));
        }

        [Test]
        public void ConfigSourceCanHaveVariablesSetAtRuntime()
        {
            DoConsoleTest(new Dictionary<string, string>
            {
                { "OneConfig_Source", "this file" },
                { "OneConfig_Source2", "Data Source=#{source};Initial Catalog=#{database};Integrated Security=True" },
                { "source", @"localhost\SQLExpress" }
            },
                new string[] {  "?SampleDBKey",
                                "database=MyDatabase",
                                "?SampleDBKey"
                },
                str => Assert.AreEqual(String.Empty, str),
                str => Assert.AreEqual("SampleDBValue", str)                
            );
        }
    }
}
