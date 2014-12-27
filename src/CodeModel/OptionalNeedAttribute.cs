using System;

namespace CodeModel
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class OptionalNeedAttribute : Attribute
    {
        public string[] OptionalNeeds { get; private set; }

        public OptionalNeedAttribute(params string[] optionalNeeds)
        {
            OptionalNeeds = optionalNeeds;
        }
    }
}