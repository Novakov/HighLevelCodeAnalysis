using System;

namespace CodeModel.RuleEngine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ViolationAttribute : Attribute
    {
        public string DisplayText { get; set; }
    }
}