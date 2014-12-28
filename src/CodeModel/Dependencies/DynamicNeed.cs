using System;

namespace CodeModel.Dependencies
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DynamicNeed : Attribute
    {
        
    }
}