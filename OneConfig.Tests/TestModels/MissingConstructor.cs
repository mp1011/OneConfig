using OneConfig.Models;
using OneConfig.Services.Interfaces;
using System;

namespace OneConfig.Tests.TestModels
{
    [IdentifyByString("bad ctor")]
    class MissingConstructor : IConfigurationReader
    {
        public MissingConstructor(string noPublicParameterlessConstructor)
        {

        }

        public string GetSingleValue(string key)
        {
            throw new NotImplementedException();
        }
    }
}
