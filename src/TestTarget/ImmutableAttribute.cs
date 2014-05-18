using System;

namespace TestTarget
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ImmutableAttribute : Attribute
    {
    }
}