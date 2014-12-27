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
}