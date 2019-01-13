using System;

namespace OneConfig.Models
{
    public class IdentifyByStringAttribute : Attribute
    {
        public string Name { get; }

        public IdentifyByStringAttribute(string name)
        {
            Name = name;
        }
    }
}
