using System;

namespace OneConfig.Models.Exceptions
{
    public class RequiredValueNotFoundException : Exception
    {
        public RequiredValueNotFoundException(string key) : base($"Required variable {key} was not found") { }
    }
}
