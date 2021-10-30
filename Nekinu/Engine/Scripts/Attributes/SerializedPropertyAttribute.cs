using System;

namespace Nekinu
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class SerializedPropertyAttribute : Attribute { }
}