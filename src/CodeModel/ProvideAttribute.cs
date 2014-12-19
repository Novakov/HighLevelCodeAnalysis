using System;

namespace CodeModel
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ProvideAttribute : Attribute
    {
        public string[] Provides { get; private set; }

        public ProvideAttribute(params string[] provides)
        {
            Provides = provides;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NeedAttribute : Attribute
    {
        public string[] Needs { get; private set; }

        public NeedAttribute(params string[] needs)
        {
            Needs = needs;
        }
    }
}