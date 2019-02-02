using System;
using System.Linq;

namespace OneConfig.ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach(var arg in args)
            {
                if(arg.Contains('='))
                {
                    var nameValue = arg.Split('=');
                    AppConfig.SetValue(nameValue[0], nameValue[1]);
                }
                else if(arg.StartsWith("??"))
                {
                    var key = arg.Substring(2);
                    Console.WriteLine($"Value {key} = {AppConfig.GetValue(key)} from {AppConfig.GetValueSource(key).Description}");
                }
                else if (arg.StartsWith("?"))
                {
                    var key = arg.Substring(1);
                    var value = AppConfig.GetValue(key);
                    Console.WriteLine(value);
                }
            }
        }
    }
}
