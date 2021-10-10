using System;

namespace Nekinu.Editor
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class UpdateInEditorAttribute : Attribute
    {
    }
}
